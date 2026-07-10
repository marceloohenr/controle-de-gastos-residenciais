import { createBrowserRouter,RouterProvider } from 'react-router-dom';import { Layout } from './components/Layout';import { Dashboard } from './pages/Dashboard';import { People } from './pages/People';import { Transactions } from './pages/Transactions';import { SummaryPage } from './pages/Summary';
const router=createBrowserRouter([{path:'/',element:<Layout/>,children:[{index:true,element:<Dashboard/>},{path:'people',element:<People/>},{path:'transactions',element:<Transactions/>},{path:'summary',element:<SummaryPage/>}]}]);

/** Inicializa o roteamento das páginas dentro do layout principal. */
export default function App(){return <RouterProvider router={router}/>}
