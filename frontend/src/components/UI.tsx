import type { ReactNode } from 'react';
export function PageHeader({eyebrow,title,children}:{eyebrow:string;title:string;children?:ReactNode}){return <header className="page-header"><div><span>{eyebrow}</span><h1>{title}</h1></div>{children}</header>}
export function Skeleton(){return <div className="skeleton" aria-label="Carregando"><i/><i/><i/></div>}
export function Empty({title,text}:{title:string;text:string}){return <div className="empty"><strong>{title}</strong><p>{text}</p></div>}
export function ErrorMessage({message}:{message:string}){return <div className="error-box" role="alert">{message}</div>}
export function StatCard({label,value,tone='neutral'}:{label:string;value:string;tone?:string}){return <article className={`stat ${tone}`}><span>{label}</span><strong>{value}</strong></article>}
