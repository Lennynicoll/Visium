import { NavLink, Outlet } from 'react-router-dom';
import './Layout.css';

export default function Layout() {
  return (
    <div>
      <header className="header">
        <h1>Visium</h1>
        <nav>
          <NavLink to="/">Dashboard</NavLink>
          <NavLink to="/visitantes">Visitantes</NavLink>
          <NavLink to="/anfitriones">Anfitriones</NavLink>
          <NavLink to="/visitas">Visitas</NavLink>
          <NavLink to="/oficinas">Oficinas</NavLink>
          <NavLink to="/motivos">Motivos</NavLink>
          <NavLink to="/departamentos">Departamentos</NavLink>
          <NavLink to="/documentos">Documentos</NavLink>
        </nav>
      </header>
      <main className="main">
        <Outlet />
      </main>
    </div>
  );
}
