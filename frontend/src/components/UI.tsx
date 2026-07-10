import type { ReactNode } from 'react';

/** Apresenta o contexto e o título padronizados de cada página. */
export function PageHeader({eyebrow,title,children}:{eyebrow:string;title:string;children?:ReactNode}){return <header className="page-header"><div><span>{eyebrow}</span><h1>{title}</h1></div>{children}</header>}

/** Exibe uma representação temporária do conteúdo durante o carregamento. */
export function Skeleton(){return <div className="skeleton" aria-label="Carregando"><i/><i/><i/></div>}

/** Padroniza a orientação exibida quando uma consulta não possui resultados. */
export function Empty({title,text}:{title:string;text:string}){return <div className="empty"><strong>{title}</strong><p>{text}</p></div>}

/** Exibe falhas de consulta em uma região anunciada por tecnologias assistivas. */
export function ErrorMessage({message}:{message:string}){return <div className="error-box" role="alert">{message}</div>}

/** Apresenta um indicador financeiro com variação visual por categoria. */
export function StatCard({label,value,tone='neutral'}:{label:string;value:string;tone?:string}){return <article className={`stat ${tone}`}><span>{label}</span><strong>{value}</strong></article>}
