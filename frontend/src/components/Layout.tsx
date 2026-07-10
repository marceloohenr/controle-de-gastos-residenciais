import { BarChart3, House, ReceiptText, Users } from 'lucide-react';
import { NavLink, Outlet } from 'react-router-dom';
const links=[['/','Visão geral',House],['/people','Pessoas',Users],['/transactions','Transações',ReceiptText],['/summary','Totais',BarChart3]] as const;

/** Estrutura a navegação responsiva e renderiza a página correspondente à rota ativa. */
export function Layout(){return <div className="shell"><aside><div className="brand"><span>lar.</span>finanças</div><nav>{links.map(([to,label,Icon])=><NavLink key={to} to={to} end={to==='/'}><Icon size={19}/><span>{label}</span></NavLink>)}</nav><small>Controle residencial</small></aside><main><Outlet/></main></div>}
