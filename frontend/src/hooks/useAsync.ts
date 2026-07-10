import { useCallback, useEffect, useState } from 'react';
export function useAsync<T>(load:()=>Promise<T>, dependencies:unknown[]=[]){
 const [data,setData]=useState<T>();const [loading,setLoading]=useState(true);const [error,setError]=useState('');
 // O chamador informa as dependências que alteram a consulta; a função é recriada durante a renderização.
 // eslint-disable-next-line react-hooks/exhaustive-deps
 const refresh=useCallback(async()=>{setLoading(true);setError('');try{setData(await load())}catch(e){setError(e instanceof Error?e.message:'Erro inesperado')}finally{setLoading(false)}},dependencies);
 useEffect(()=>{void refresh()},[refresh]);return{data,loading,error,refresh};
}
