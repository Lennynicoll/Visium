import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import Visitantes from './pages/Visitantes';
import Anfitriones from './pages/Anfitriones';
import Visitas from './pages/Visitas';
import Oficinas from './pages/Oficinas';
import MotivosVisita from './pages/MotivosVisita';
import Departamentos from './pages/Departamentos';
import Documentos from './pages/Documentos';

export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<Dashboard />} />
        <Route path="/visitantes" element={<Visitantes />} />
        <Route path="/anfitriones" element={<Anfitriones />} />
        <Route path="/visitas" element={<Visitas />} />
        <Route path="/oficinas" element={<Oficinas />} />
        <Route path="/motivos" element={<MotivosVisita />} />
        <Route path="/departamentos" element={<Departamentos />} />
        <Route path="/documentos" element={<Documentos />} />
      </Route>
    </Routes>
  );
}
