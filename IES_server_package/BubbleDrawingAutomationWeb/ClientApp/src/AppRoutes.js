import { Counter } from "./components/Pages/Counter";
import { FetchData } from "./components/Pages/FetchData";
import { Home } from "./components/Pages/Home";
import  Login  from "./components/Login/Login";

const AppRoutes = [
  {
    index: true,
    element: <Home />
    },
  {
    path: '/counter',
    element: <Counter />
    },
    {
        path: '/login',
        element: <Login />
    },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
