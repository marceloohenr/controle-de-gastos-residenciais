import type { Person, Summary, Transaction, TransactionType } from '../types';
const baseUrl = import.meta.env.VITE_API_URL ?? 'http://localhost:5080/api';
async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${baseUrl}${path}`, { ...options, headers: { 'Content-Type': 'application/json', ...options?.headers } });
  if (!response.ok) { const problem = await response.json().catch(() => null); throw new Error(problem?.detail ?? 'Não foi possível concluir a operação.'); }
  return response.status === 204 ? undefined as T : response.json();
}
export const api = {
  people: { list: (search = '') => request<Person[]>(`/people${search ? `?search=${encodeURIComponent(search)}` : ''}`), create: (body: {name:string;age:number}) => request<Person>('/people',{method:'POST',body:JSON.stringify(body)}), remove: (id:number) => request<void>(`/people/${id}`,{method:'DELETE'}) },
  transactions: { list: () => request<Transaction[]>('/transactions'), create: (body:{description:string;amount:number;type:TransactionType;personId:number}) => request<Transaction>('/transactions',{method:'POST',body:JSON.stringify(body)}) },
  summary: () => request<Summary>('/summary')
};
