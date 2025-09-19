import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { NgIconsModule } from '@ng-icons/core';
import { map } from 'rxjs/operators';

// Importar tipos do serviço auditoria-api
import type { StockQueryResult, AuditSaveResult } from '../../services/auditoria-api';
import { AuditoriaApiService } from '../../services/auditoria-api';

// Importar tipos DTOs do Supabase
import type { FranchiseDto, TechnicianDto, SupabaseDealDto } from '../../models/supabase-dtos';

// Importar tipos específicos do formulário de auditoria do novo arquivo
import type { DealsProcessadas, ObraNaoFornecida, AuditDataForm } from '../../models/audit-form.model';


@Component({
  selector: 'app-abertura-auditoria',
  standalone: true,
  imports: [CommonModule, FormsModule, NgIconsModule],
  templateUrl: './abertura-auditoria.html',
  styleUrl: './abertura-auditoria.scss',
})
export class AberturaAuditoria implements OnInit, OnDestroy {
  loading = false;
  transmitindo = false;

  teams: FranchiseDto[] = [];
  teamSelecionado: FranchiseDto | null = null;
  usuarioSelecionado: FranchiseDto | null = null;

  obrasProcessadas: DealsProcessadas[] = [];

  dadosCampo: Record<string, any> = {};
  dadosSistema: Record<string, any> = {};

  sistemaEditavel = false;
  status = "";
  tecnicoSelecionado: number | null = null;
  technicians: TechnicianDto[] = [];

  erro: string | null = null;
  sucesso: string | null = null;
  statusConexao: { connected: boolean; message: string; totalTeams?: number } | null = { connected: true, message: "Conexão OK com Supabase", totalTeams: 0 };


  imagensSelecionadas: File[] = [];
  previewsImagens: string[] = [];
  nomesImagens: string[] = [];

  contagemM2CampoPorOrdem: { [orderId: number]: string } = {};
  observacoesPorOrdem: { [orderId: number]: string } = {};
  obrasNaoFornecidas: ObraNaoFornecida[] = [
    { contagemM2: "", cliente: "", endereco: "", periodo: "" },
  ];
  cnpjFranquia: string | null | undefined = null;
  razaoSocialFranquia: string | null | undefined = null;
  enderecoFranquia: string | null | undefined = null;

  mostrarAvisoContagem = false;

  auditType: "live" | "past" = "live";
  selectedPastDate: string = "";
  loadingObras = false;

  private subscriptions: Subscription = new Subscription();

  auditData: AuditDataForm = {};


  constructor(private auditoriaApi: AuditoriaApiService) {}

  ngOnInit(): void {
    this.carregarFranquias();
    this.carregarTechnicians();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  get m2InstaladosCampoTotal(): number {
    const m2Ordens = Object.values(this.contagemM2CampoPorOrdem).reduce((sum, val) => sum + (Number(val) || 0), 0);
    const m2NaoFornecidas = this.obrasNaoFornecidas.reduce((sum, obra) => sum + (Number(obra.contagemM2) || 0), 0);
    return m2NaoFornecidas + m2Ordens;
  }

  get somaCampoEstoqueDisplay(): string {
    const estoqueOcioso = Number(this.dadosCampo['estoqueOcioso'] || 0);
    const soma = this.m2InstaladosCampoTotal + estoqueOcioso;
    return soma.toString();
  }

  carregarFranquias(): void {
    this.loading = true;
    this.setErro(null);
    this.subscriptions.add(
      this.auditoriaApi.getFranchises().subscribe({
        next: (data: FranchiseDto[]) => {
          this.teams = data.sort((a, b) => (a.name || '').localeCompare(b.name || '', "pt-BR"));
          if (this.statusConexao) {
            this.statusConexao.totalTeams = data.length;
          }
        },
        error: (err: any) => {
          console.error("Erro ao carregar franquias do Supabase:", err);
          this.setErro(`Erro ao carregar franquias: ${err.message}`);
          if (this.statusConexao) {
            this.statusConexao.connected = false;
            this.statusConexao.message = `Falha ao carregar franquias: ${err.message}`;
          }
        },
        complete: () => this.loading = false
      })
    );
  }

trackByObraNaoFornecida(index: number, obra: ObraNaoFornecida): number {
    return index;
  }

  carregarTechnicians(): void {
    this.loading = true;
    this.setErro(null);
    this.subscriptions.add(
      this.auditoriaApi.getTechnicians().subscribe({
        next: (data: TechnicianDto[]) => this.technicians = data,
        error: (err: any) => {
          this.erro = "Falha ao carregar técnicos: " + err.message;
          console.error("Erro ao carregar técnicos:", err);
          if (this.statusConexao) {
            this.statusConexao.connected = false;
            this.statusConexao.message = `Falha ao carregar técnicos: ${err.message}`;
          }
        },
        complete: () => this.loading = false
      })
    );
  }

  selecionarTeam(teamName: string): void {
    const team = this.teams.find(t => t.name === teamName);
    if (!team) {
      this.setErro("Franquia não encontrada.");
      return;
    }

    this.loading = true;
    this.setErro(null);
    this.teamSelecionado = team;
    this.usuarioSelecionado = team;

    this.obrasProcessadas = [];
    this.dadosSistema = {};
    this.dadosCampo = {};

    this.auditData.franqueadoId = team.id.toString();
    this.auditData.unidadeFranqueada = team.name;
    this.auditData.proprietario = team.owner?.toString() || "Não informado";
    this.auditData.email = team.email;
    this.auditData.whatsapp = team.whatsapp;
    this.auditData.cnpj = team.cnpj;
    this.auditData.razaoSocial = team.corporateName;
    this.auditData.endereco = team.address;
    this.auditData.cidade = team.city;
    this.auditData.estado = team.state;
    this.auditData.enderecoEstoque = "Não informado";


    this.cnpjFranquia = team.cnpj;
    this.razaoSocialFranquia = team.corporateName;
    this.enderecoFranquia = team.address;


    if (this.auditData.cnpj) {
      this.auditoriaApi.getStockByCnpj(this.auditData.cnpj).subscribe({
        next: ({ quantidade, erro }) => {
          if (erro) {
            this.setErro(`Erro ao buscar estoque: ${erro}`);
          } else {
            this.dadosSistema['quantidadeEstoque'] = quantidade.toString();
          }
        },
        error: (err) => this.setErro(`Erro ao buscar estoque: ${err.message}`)
      });
    }

    this.fetchObras();
    this.loading = false;
  }

  fetchObras(): void {
    if (!this.teamSelecionado) {
      this.obrasProcessadas = [];
      return;
    }

    if (this.auditType === "live" && !this.teamSelecionado.id) {
      console.log("Aguardando ID da franquia para auditoria em tempo real...");
      this.obrasProcessadas = [];
      return;
    }

    if (this.auditType === "past" && (!this.selectedPastDate || !this.teamSelecionado.code)) {
      console.log("Aguardando seleção de data e/ou código da franquia para auditoria atemporal...");
      this.obrasProcessadas = [];
      return;
    }

    this.loadingObras = true;
    this.setErro(null);
    this.obrasProcessadas = [];
    this.contagemM2CampoPorOrdem = {};
    this.observacoesPorOrdem = {};

    let obrasObservable: Observable<DealsProcessadas[]>;
    let franchiseId: number = this.teamSelecionado.id;
    let selectedDate: string | null = null;

    if (this.auditType === "past") {
        selectedDate = this.selectedPastDate;
    }

    console.log(`🔄 Buscando obras (Franquia ID: ${franchiseId}, Data: ${selectedDate || 'atual'})`);
    obrasObservable = this.auditoriaApi.getDeals(franchiseId, selectedDate).pipe(
        map((deals: SupabaseDealDto[]) => deals.map((deal: SupabaseDealDto) => {
            if (this.auditType === "live") {
                const lastUpdate = new Date(deal.lastUpdateDate || deal.createdAt);
                const period = deal.periodo || 0;
                const endDate = new Date(lastUpdate.getTime() + period * 24 * 60 * 60 * 1000);
                deal.expirada = endDate < new Date();
            }
            return deal;
        }))
    );


    this.subscriptions.add(
      obrasObservable.subscribe({
        next: (obras: DealsProcessadas[]) => {
          this.obrasProcessadas = this.auditType === "live"
            ? (obras as SupabaseDealDto[]).filter(o => !o.expirada)
            : obras;

          if (this.obrasProcessadas.length > 0) {
            const m2InstaladosSistema = this.obrasProcessadas.reduce((acc, obra) => acc + (Number(obra.m2Instalados) || 0), 0);
            this.dadosSistema['m2Instalados'] = m2InstaladosSistema.toString();
          } else {
            this.dadosSistema['m2Instalados'] = "0";
          }
        },
        error: (err: any) => {
          console.error("❌ Erro ao carregar obras:", err);
          this.setErro(`Erro ao carregar obras: ${err.message}`);
          if (this.statusConexao) {
            this.statusConexao.connected = false;
            this.statusConexao.message = `Falha ao carregar obras: ${err.message}`;
          }
        },
        complete: () => this.loadingObras = false
      })
    );
  }

  private extrairCNPJUsuario(franquia: FranchiseDto): string | null | undefined {
    return franquia.cnpj || null;
  }

  private extrairRazaoSocialUsuario(franquia: FranchiseDto): string | null | undefined {
    return franquia.corporateName || null;
  }

  private extrairEnderecoUsuario(franquia: FranchiseDto): string | null | undefined {
    return franquia.address || null;
  }

  atualizarEstoqueOcioso(): void {
    const quantidadeEstoque = Number(this.dadosSistema['quantidadeEstoque'] || 0);
    const m2Instalados = Number(this.dadosSistema['m2Instalados'] || 0);
    const estoqueOciosoCalculado = quantidadeEstoque - m2Instalados;
    this.dadosSistema = {
      ...this.dadosSistema,
      estoqueOcioso: estoqueOciosoCalculado >= 0 ? estoqueOciosoCalculado.toString() : "0",
    };
  }

  handleCampoChange(campo: string, valor: any): void {
    this.dadosCampo[campo] = valor;
  }

  handleSistemaChange(campo: string, valor: any): void {
    this.dadosSistema[campo] = valor;
    if (campo === 'quantidadeEstoque' || campo === 'm2Instalados') {
      this.atualizarEstoqueOcioso();
    }
  }

  handleContagemM2Change(orderId: number, valor: string): void {
    this.contagemM2CampoPorOrdem = { ...this.contagemM2CampoPorOrdem, [orderId]: valor };
  }

  handleObservacaoChange(orderId: number, valor: string): void {
    this.observacoesPorOrdem = { ...this.observacoesPorOrdem, [orderId]: valor };
  }

  handleObraNaoFornecidaChange(index: number, campo: keyof ObraNaoFornecida, valor: any): void {
    this.obrasNaoFornecidas[index][campo] = valor;

    const obraAtual = this.obrasNaoFornecidas[index];
    const temAlgumCampoPreenchido = obraAtual.contagemM2 || obraAtual.cliente || obraAtual.endereco || obraAtual.periodo;
    const eUltimaObra = index === this.obrasNaoFornecidas.length - 1;

    if (temAlgumCampoPreenchido && eUltimaObra) {
      this.obrasNaoFornecidas.push({ contagemM2: "", cliente: "", endereco: "", periodo: "" });
    }
    if (!temAlgumCampoPreenchido && !eUltimaObra && this.obrasNaoFornecidas.length > 1) {
        this.obrasNaoFornecidas.splice(index, 1);
    }
  }

  removerObraNaoFornecida(index: number): void {
    if (this.obrasNaoFornecidas.length > 1) {
      this.obrasNaoFornecidas.splice(index, 1);
    }
  }

  handleImagensChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const arquivos = Array.from(input.files || []);
    if (arquivos.length === 0) return;

    const tiposPermitidos = ["image/jpeg", "image/jpg", "image/png"];
    const arquivosValidos: File[] = [];
    const nomesNovos: string[] = [];

    for (const arquivo of arquivos) {
      if (!tiposPermitidos.includes(arquivo.type)) {
        this.setErro(`Arquivo ${arquivo.name}: Apenas JPG, JPEG e PNG são permitidos!`);
        continue;
      }
      if (arquivo.size > 5 * 1024 * 1024) {
        this.setErro(`Arquivo ${arquivo.name}: Deve ter no máximo 5MB!`);
        continue;
      }
      arquivosValidos.push(arquivo);
      nomesNovos.push(arquivo.name);
    }

    const totalImagens = this.imagensSelecionadas.length + arquivosValidos.length;
    if (totalImagens > 100) {
      this.setErro("Máximo de 100 imagens permitidas!");
      return;
    }

    arquivosValidos.forEach((arquivo) => {
      const reader = new FileReader();
      reader.onload = (e) => {
        const preview = e.target?.result as string;
        this.previewsImagens = [...this.previewsImagens, preview];
      };
      reader.readAsDataURL(arquivo);
    });

    this.imagensSelecionadas = [...this.imagensSelecionadas, ...arquivosValidos];
    this.nomesImagens = [...this.nomesImagens, ...nomesNovos];
    this.setErro(null);
  }

  removerImagem(index: number): void {
    this.imagensSelecionadas.splice(index, 1);
    this.previewsImagens.splice(index, 1);
    this.nomesImagens.splice(index, 1);
  }

  abrirDocumentoPdf(url: string): void {
    window.open(url, '_blank');
  }

  async handleTransmit(): Promise<void> {
    if (!this.teamSelecionado) {
      this.setErro("Selecione uma franquia primeiro!");
      return;
    }
    if (this.tecnicoSelecionado === null) {
      this.setErro("Selecione um técnico responsável!");
      return;
    }

    if (this.auditType === "past" && !this.selectedPastDate) {
      this.setErro("Selecione uma data para a auditoria atemporal!");
      return;
    }

    const ordensComContagemVazia = this.obrasProcessadas.filter((obra) => !this.contagemM2CampoPorOrdem[obra.id]);
    if (ordensComContagemVazia.length > 0 && !this.mostrarAvisoContagem) {
      this.setMostrarAvisoContagem(true);
      return;
    }

    let imagensBase64: string[] = [];
    if (this.imagensSelecionadas.length > 0) {
      try {
        imagensBase64 = await Promise.all(this.imagensSelecionadas.map((arquivo) => this.convertImageToBase64(arquivo)));
      } catch (error) {
        this.setErro("Erro ao processar imagens!");
        return;
      }
    }

    this.auditData.franqueadoId = this.teamSelecionado.id.toString();
    this.auditData.unidadeFranqueada = this.teamSelecionado.name;
    this.auditData.proprietario = this.usuarioSelecionado?.name || "Não informado";
    this.auditData.email = this.usuarioSelecionado?.email;
    this.auditData.whatsapp = this.usuarioSelecionado?.whatsapp;
    this.auditData.cnpj = this.cnpjFranquia;
    this.auditData.razaoSocial = this.razaoSocialFranquia;
    this.auditData.endereco = this.enderecoFranquia;
    this.auditData.cidade = this.usuarioSelecionado?.city;
    this.auditData.estado = this.usuarioSelecionado?.state;
    this.auditData.enderecoEstoque = "Não informado";

    this.auditData.technicianId = this.tecnicoSelecionado;
    this.auditData.status = this.status;

    this.auditData.dataVisitaTecnica = this.dadosCampo['dataVisitaTecnica'];
    this.auditData.m2InstaladosCampo = this.m2InstaladosCampoTotal.toString();
    this.auditData.estoqueOciosoCampo = this.dadosCampo['estoqueOcioso'];
    this.auditData.placasDanificadasCampo = this.dadosCampo['placasDanificadas'];
    this.auditData.observacoesGerais = this.dadosCampo['observacoesGerais'];

    this.auditData.quantidadeEstoqueSistema = this.dadosSistema['quantidadeEstoque'];
    this.auditData.m2InstaladosSistema = this.obrasProcessadas.reduce((sum, obra) => sum + (Number(obra.m2Instalados) || 0), 0).toString();
    this.auditData.estoqueOciosoSistema = (Number(this.dadosSistema['quantidadeEstoque'] || 0) - Number(this.auditData.m2InstaladosSistema || 0)).toString();

    this.auditData.images = imagensBase64.length > 0 ? imagensBase64 : null;
    this.auditData.imageNames = this.nomesImagens.length > 0 ? this.nomesImagens : null;

    this.auditData.obrasProcessadasIds = this.obrasProcessadas.map(obra => obra.id);
    this.auditData.contagemM2Campo = Object.values(this.contagemM2CampoPorOrdem).map(value => String(value));
    this.auditData.observacoesPorOrdemValues = Object.values(this.observacoesPorOrdem);
    this.auditData.obrasNaoFornecidasJson = this.obrasNaoFornecidas.filter(
        (obra) => obra.contagemM2 || obra.cliente || obra.endereco || obra.periodo,
    ).length > 0 ? JSON.stringify(this.obrasNaoFornecidas.filter(
        (obra) => obra.contagemM2 || obra.cliente || obra.endereco || obra.periodo,
    )) : null;

    this.auditData.customCreatedAt = this.auditType === "past" && this.selectedPastDate ? new Date(this.selectedPastDate + "T12:00:00Z") : null;


    try {
      this.transmitindo = true;
      this.setErro(null);
      this.setSucesso(null);

      this.subscriptions.add(
        this.auditoriaApi.submitAudit(this.auditData).subscribe({
          next: (resultado: AuditSaveResult) => {
            if (resultado.success) {
              this.setSucesso("Auditoria transmitida e salva com sucesso!");
              this.resetForm();
            } else {
              this.setErro(resultado.message || `Erro ao salvar auditoria.`);
            }
          },
          error: (err: any) => {
            console.error("Erro na transmissão da auditoria:", err);
            this.setErro(`Falha na transmissão: ${err.message}`);
          },
          complete: () => this.transmitindo = false
        })
      );
    } catch (error: any) {
      console.error("Exceção na transmissão da auditoria:", error);
      this.setErro(`Falha na transmissão (catch): ${error.message}`);
      this.transmitindo = false;
    }
  }

  private convertImageToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (e) => resolve(e.target?.result as string);
      reader.onerror = (error) => reject(error);
    });
  }

  verificarSomaEstoque(): boolean {
    const quantidadeEstoqueSistemaStr = this.dadosSistema['quantidadeEstoque'];
    if (!quantidadeEstoqueSistemaStr) return true;
    const m2Instalados = Number.parseFloat(this.m2InstaladosCampoTotal?.toString() || "0");
    const estoqueOcioso = Number(this.dadosCampo['estoqueOcioso'] || 0);
    const quantidadeEstoqueSistema = Number.parseFloat(quantidadeEstoqueSistemaStr.toString());
    if (isNaN(m2Instalados) || isNaN(estoqueOcioso) || isNaN(quantidadeEstoqueSistema)) return false;
    const somaCampo = m2Instalados + estoqueOcioso;
    const epsilon = 0.001;
    return Math.abs(somaCampo - quantidadeEstoqueSistema) < epsilon;
  }

  verificarSomaOcioso(): boolean {
    if (this.dadosCampo['estoqueOcioso'] === undefined || this.dadosCampo['estoqueOcioso'] === null) return true;
    const placasDanificadas = Number(this.dadosCampo['placasDanificadas'] || 0);
    const estoqueOcioso = Number(this.dadosCampo['estoqueOcioso']);
    return placasDanificadas <= estoqueOcioso;
  }

  formatarData(dataString?: string | null): string {
    if (!dataString) return "Não informado";
    try {
      const data = new Date(dataString.endsWith('Z') ? dataString : `${dataString}Z`);
      if (isNaN(data.getTime())) return "Data inválida";
      return data.toLocaleDateString("pt-BR", { timeZone: "UTC" });
    } catch (e) {
      return "Data inválida";
    }
  }

  formatarValor(valor?: number | null): string {
    if (valor === undefined || valor === null) return "Não informado";
    return valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
  }

  getNomeTecnicoSelecionado(): string {
    if (this.tecnicoSelecionado === null) return "";
    const tecnico = this.technicians.find((t) => t.id === this.tecnicoSelecionado);
    return tecnico?.name || "";
  }

  setErro(message: string | null): void {
    this.erro = message;
    if (message) {
      this.setSucesso(null);
    }
  }

  setSucesso(message: string | null): void {
    this.sucesso = message;
    if (message) {
      this.setErro(null);
    }
  }

  setAuditType(type: "live" | "past"): void {
    this.auditType = type;
    this.fetchObras();
  }

  setSelectedPastDate(date: string): void {
    this.selectedPastDate = date;
    this.fetchObras();
  }

  setMostrarAvisoContagem(value: boolean): void {
    this.mostrarAvisoContagem = value;
  }

  private resetForm(): void {
    this.dadosCampo = {};
    this.dadosSistema = {};
    this.status = "";
    this.tecnicoSelecionado = null;
    this.sistemaEditavel = false;
    this.teamSelecionado = null;
    this.usuarioSelecionado = null;
    this.obrasProcessadas = [];
    this.imagensSelecionadas = [];
    this.previewsImagens = [];
    this.nomesImagens = [];
    this.contagemM2CampoPorOrdem = {};
    this.observacoesPorOrdem = {};
    this.obrasNaoFornecidas = [{ contagemM2: "", cliente: "", endereco: "", periodo: "" }];
    this.cnpjFranquia = null;
    this.razaoSocialFranquia = null;
    this.enderecoFranquia = null;
    this.mostrarAvisoContagem = false;
    this.auditType = "live";
    this.selectedPastDate = "";
    this.auditData = {
    };
  }
}