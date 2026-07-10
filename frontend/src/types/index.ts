export type TransactionType = 'Income' | 'Expense';
export interface Person { id: number; name: string; age: number; createdAt: string }
export interface Transaction { id: number; description: string; amount: number; type: TransactionType; personId: number; personName: string; createdAt: string }
export interface PersonSummary { personId: number; personName: string; totalIncome: number; totalExpenses: number; balance: number }
export interface Summary { people: PersonSummary[]; totals: { totalIncome: number; totalExpenses: number; netBalance: number } }
