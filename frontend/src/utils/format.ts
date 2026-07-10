export const money = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format;
export const date = (value: string) => new Intl.DateTimeFormat('pt-BR').format(new Date(value));
