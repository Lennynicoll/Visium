import { useState, useEffect } from 'react';
import * as visitanteService from '../api/visitanteService';
import * as anfitrionService from '../api/anfitrionService';
import * as visitaService from '../api/visitaService';
import * as motivoService from '../api/motivoService';

export default function Dashboard() {
  const [stats, setStats] = useState({ visitantes: 0, anfitriones: 0, visitas: 0, enCurso: 0, motivos: 0 });
  const [error, setError] = useState('');

  const loadStats = () => {
    Promise.all([
      visitanteService.getAll(),
      anfitrionService.getAll(),
      visitaService.getAll(),
      motivoService.getAll(),
    ])
      .then(([visitantes, anfitriones, visitas, motivos]) => {
        setStats({
          visitantes: visitantes.data.length,
          anfitriones: anfitriones.data.length,
          visitas: visitas.data.length,
          enCurso: visitas.data.filter((v) => v.estado === 'En Curso').length,
          motivos: motivos.data.length,
        });
      })
      .catch((err) => setError(err.message));
  };

  useEffect(() => {
    loadStats();
    const interval = setInterval(loadStats, 5000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div>
      <h1>Dashboard</h1>
      {error && <div className="error">{error}</div>}

      <div className="stats">
        <div className="stat-card">
          <h3>Visitantes</h3>
          <p>{stats.visitantes}</p>
        </div>
        <div className="stat-card">
          <h3>Anfitriones</h3>
          <p>{stats.anfitriones}</p>
        </div>
        <div className="stat-card">
          <h3>Visitas registradas</h3>
          <p>{stats.visitas}</p>
        </div>
        <div className="stat-card">
          <h3>Visitas en curso</h3>
          <p>{stats.enCurso}</p>
        </div>
      </div>
      <div className="stats">
        <div className="stat-card">
          <h3>Motivos de visita</h3>
          <p>{stats.motivos}</p>
        </div>
      </div>
    </div>
  );
}
