/** Formata valores monetários usando Real e as convenções do português brasileiro. */
export const money = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format;

/** Converte datas da API para a representação curta do português brasileiro. */
export const date = (value: string) => new Intl.DateTimeFormat('pt-BR').format(new Date(value));
