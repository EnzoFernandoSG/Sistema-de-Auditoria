import type { SupabaseDealDto } from './supabase-dtos';

export type DealsProcessadas = SupabaseDealDto;

export interface ObraNaoFornecida { 
  contagemM2: string | number | null | undefined;
  cliente: string | null | undefined;
  endereco: string | null | undefined;
  periodo: string | number | null | undefined;
}

export interface DadosCampoUi { 
  dataVisitaTecnica?: string | null;
  m2Instalados?: string | null;
  estoqueOcioso?: number | null;
  placasDanificadas?: number | null;
  observacoesGerais?: string | null;
}

export interface DadosSistemaUi { 
  quantidadeEstoque?: string | null;
  m2Instalados?: string | null;
  estoqueOcioso?: string | null;
}

// Interface para o formato do dado de auditoria que o frontend ENVIA ao backend
export interface AuditDataForm {
  franqueadoId?: string | null | undefined;
  unidadeFranqueada?: string | null | undefined;
  proprietario?: string | null | undefined;
  email?: string | null | undefined;
  enderecoEstoque?: string | null | undefined;
  cidade?: string | null | undefined;
  estado?: string | null | undefined;
  whatsapp?: string | null | undefined;
  cnpj?: string | null | undefined;
  razaoSocial?: string | null | undefined;
  endereco?: string | null | undefined;
  technicianId?: number | null | undefined;
  images?: string[] | null | undefined;
  imageNames?: string[] | null | undefined;

  // Campos de Dados de Campo - AGORA DIRETOS NO AUDITDATAFORM
  dataVisitaTecnica?: string | null | undefined;
  m2InstaladosCampo?: string | null | undefined;
  estoqueOciosoCampo?: number | null | undefined;
  placasDanificadasCampo?: number | null | undefined;
  observacoesGerais?: string | null | undefined;

  // Campos de Dados de Sistema - AGORA DIRETOS NO AUDITDATAFORM
  quantidadeEstoqueSistema?: string | null | undefined;
  m2InstaladosSistema?: string | null | undefined;
  estoqueOciosoSistema?: string | null | undefined;


  status?: string | null | undefined;
  obrasProcessadasIds?: Array<number> | null | undefined;
  contagemM2Campo?: string[] | null | undefined;
  observacoesPorOrdemValues?: string[] | null | undefined;

  obrasNaoFornecidasJson?: string | null | undefined;

  customCreatedAt?: Date | null | undefined;
}