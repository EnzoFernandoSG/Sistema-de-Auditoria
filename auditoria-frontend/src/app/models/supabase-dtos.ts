// Interfaces para os DTOs que vêm do backend .NET do Supabase
export interface FranchiseDto {
  id: number;
  name: string;
  code: string;
  email?: string | null; // Alterado para 'null'
  cnpj?: string | null; // Alterado para 'null'
  state?: string | null; // Alterado para 'null'
  city?: string | null; // Alterado para 'null'
  owner?: number | null; // foreign key para Users
  whatsapp?: string | null; // Alterado para 'null'
  address?: string | null; // Alterado para 'null'
  district?: string | null; // Alterado para 'null'
  website?: string | null; // Alterado para 'null'
  createdAt?: string | null; // DateOnly mapeia para string no frontend. Alterado para 'null'
  note?: string | null; // Alterado para 'null'
  corporateName?: string | null; // Alterado para 'null'
  zipCode?: string | null; // Alterado para 'null'
  instagram?: string | null; // Alterado para 'null'
  type?: string | null; // Alterado para 'null'
  idPloomes?: string | null; // Alterado para 'null'
}

export interface TechnicianDto {
  id: number; // bigint mapeia para number
  name: string;
  phone?: string | null;
  createdAt?: string | null; // timestamp with time zone
  cpf?: string | null;
  rg?: string | null;
  born?: string | null; // DateOnly mapeia para string
}

export interface ProductDto {
  id: number; // bigint
  name?: string | null;
  unitPrice?: number | null; // double precision
  total?: number | null; // double precision
  quantity?: number | null; // double precision
  dealId?: number | null; // bigint
  currencyId?: number | null; // integer
  createdAt?: string | null; // timestamp with time zone
  productName?: string | null; // Alias. Alterado para 'null'
}

export interface CustomerDto {
  id: number; // bigint
  name: string;
  phone?: string | null;
  email?: string | null;
  street?: string | null;
  neighborhood?: string | null;
  number?: number | null; // bigint
  cep?: string | null;
  cnpj?: string | null;
  cpf?: string | null;
  createdAt?: string | null; // timestamp without time zone
  updatedAt?: string | null; // timestamp without time zone
  userId?: number | null; // bigint
  franchiseCod?: string | null;
  companyName?: string | null;
  complement?: string | null;
  city?: string | null;
  state?: string | null;
}

// DTO para representar um Deal do Supabase, formatado para o frontend
export interface SupabaseDealDto {
  id: number; // bigint
  dealId: number; // Alias para compatibilidade
  amount?: number | null; // double precision
  idCurrency?: number | null; // integer
  idCustomer?: number | null; // bigint
  idStatus?: number | null; // integer
  createdAt: string; // timestamp with time zone not null
  periodo?: number | null; // integer (renomeado de 'period')
  cep?: string | null;
  streetNumber?: string | null;
  streetName?: string | null;
  district?: string | null;
  state?: string | null;
  city?: string | null;
  documentUrl?: string | null;
  lastUpdateDate?: string | null; // timestamp without time zone
  franchiseCod?: number | null; // integer

  // Propriedades populadas no backend para o DTO
  cliente: string; // Nome do cliente
  m2Instalados: number; // M² totais instalados
  areasProtegidas: ProductDto[]; // Lista de produtos/áreas protegidas
  dataTermino?: string | null; // Data de término calculada
  status: string; // Status da obra (ex: "ativa_no_passado")
  expirada?: boolean; // Se a obra está expirada
}