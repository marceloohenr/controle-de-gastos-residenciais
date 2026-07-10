import { createContext, useCallback, useContext, useState, type ReactNode } from 'react';
type Toast = { message: string; tone: 'success' | 'error' };
const Context = createContext<(message:string,tone?:Toast['tone'])=>void>(()=>{});

/** Disponibiliza notificações transitórias de sucesso e erro para toda a aplicação. */
export function ToastProvider({children}:{children:ReactNode}) {
 const [toast,setToast]=useState<Toast|null>(null);
 const show=useCallback((message:string,tone:Toast['tone']='success')=>{setToast({message,tone});window.setTimeout(()=>setToast(null),3500)},[]);
 return <Context.Provider value={show}>{children}{toast&&<div className={`toast ${toast.tone}`} role="status">{toast.message}</div>}</Context.Provider>;
}
/** Retorna a função usada pelos fluxos da interface para exibir uma notificação. */
export const useToast=()=>useContext(Context);
