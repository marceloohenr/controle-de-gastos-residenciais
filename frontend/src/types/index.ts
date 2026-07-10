/** Tipos de movimentação aceitos pelos contratos da API. */
export type TransactionType = 'Income' | 'Expense';

/** Pessoa cadastrada e retornada pela API. */
export interface Person { id: number; name: string; age: number; createdAt: string }

/** Movimentação financeira acompanhada dos dados de identificação da pessoa. */
export interface Transaction { id: number; description: string; amount: number; type: TransactionType; personId: number; personName: string; createdAt: string }

/** Totais calculados para uma única pessoa. */
export interface PersonSummary { personId: number; personName: string; totalIncome: number; totalExpenses: number; balance: number }

/** Resultado completo da consulta de totais individuais e gerais. */
export interface Summary { people: PersonSummary[]; totals: { totalIncome: number; totalExpenses: number; netBalance: number } }
