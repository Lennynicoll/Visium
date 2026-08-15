import { useState, useEffect, useMemo } from 'react';
import CrudPage from '../components/CrudPage';
import * as visitaService from '../api/visitaService';
import * as visitanteService from '../api/visitanteService';
import * as anfitrionService from '../api/anfitrionService';
import * as departamentoService from '../api/departamentoService';
import * as motivoService from '../api/motivoService';
const fields = [
  {
    name: 'fechaHora',
    label: 'Fecha y hora programada',
    type: 'datetime-local',
    required: true,
  },
  {
    name: 'motivo',
    label: 'Motivo',
    type: 'select',
    required: true,
    loadOptions: () =>
      motivoService.getAll().then((res) => res.data.map((m) => ({ value: m.nombre, text: m.nombre }))),
  },
  { name: 'comentarios', label: 'Comentarios' },
  {
    name: 'visitanteId',
    label: 'Visitante',
    type: 'select',
    required: true,
    loadOptions: () =>
      visitanteService.getAll().then((res) =>
        res.data.map((v) => ({ value: v.id, text: `${v.nombre} ${v.apellido}` }))
      ),
  },
  {
    name: 'departamentoId',
    label: 'Departamento',
    type: 'select',
    required: true,
    loadOptions: () =>
      departamentoService.getAll().then((res) => res.data.map((d) => ({ value: d.id, text: d.nombre }))),
  },
  {
    name: 'anfitrionId',
    label: 'Anfitrión',
    type: 'select',
    required: true,
    dependeDe: 'departamentoId',
    loadOptions: (f) =>
      anfitrionService.getAll().then((res) =>
        res.data
          .filter((a) => !f.departamentoId || String(a.departamentoId) === String(f.departamentoId))
          .map((a) => ({ value: a.id, text: `${a.nombre} ${a.apellido}` }))
      ),
  },
];

export default function Visitas() {
  const [estado, setEstado] = useState('');
  const [refresh, setRefresh] = useState(0);
  const [visitantes, setVisitantes] = useState({});
  const [anfitriones, setAnfitriones] = useState({});
  const [departamentoPorAnfitrion, setDepartamentoPorAnfitrion] = useState({});

  useEffect(() => {
    visitanteService
      .getAll()
      .then((res) => setVisitantes(Object.fromEntries(res.data.map((v) => [v.id, `${v.nombre} ${v.apellido}`]))))
      .catch(() => {});
    departamentoService
      .getAll()
      .then((dres) => {
        const dMap = Object.fromEntries(dres.data.map((d) => [d.id, d.nombre]));
        anfitrionService
          .getAll()
          .then((res) => {
            setAnfitriones(Object.fromEntries(res.data.map((a) => [a.id, `${a.nombre} ${a.apellido}`])));
            setDepartamentoPorAnfitrion(
              Object.fromEntries(res.data.map((a) => [a.id, dMap[a.departamentoId] || `Depto #${a.departamentoId}`]))
            );
          })
          .catch(() => {});
      })
      .catch(() => {});
  }, []);

  const service = useMemo(
    () => ({
      getAll: () => (estado ? visitaService.getByEstado(estado) : visitaService.getAll()),
      create: visitaService.create,
      update: visitaService.update,
      remove: visitaService.remove,
    }),
    [estado]
  );

  const columns = useMemo(
    () => [
      {
        key: 'fechaHora',
        label: 'Programada',
        render: (item) => (item.fechaHora ? new Date(item.fechaHora).toLocaleString() : ''),
      },
      { key: 'motivo', label: 'Motivo' },
      {
        key: 'visitanteId',
        label: 'Visitante',
        render: (item) => visitantes[item.visitanteId] || `Visitante #${item.visitanteId}`,
      },
      {
        key: 'anfitrionId',
        label: 'Anfitrión',
        render: (item) => anfitriones[item.anfitrionId] || `Anfitrión #${item.anfitrionId}`,
      },
      {
        key: 'departamento',
        label: 'Departamento',
        render: (item) => departamentoPorAnfitrion[item.anfitrionId] || '—',
      },
      {
        key: 'estado',
        label: 'Estado',
        render: (item) => (
          <span className={`status status-${item.estado?.toLowerCase().replace(' ', '-')}`}>{item.estado}</span>
        ),
      },
      {
        key: 'fechaEntrada',
        label: 'Entrada',
        render: (item) => (item.fechaEntrada ? new Date(item.fechaEntrada).toLocaleString() : '—'),
      },
      {
        key: 'fechaSalida',
        label: 'Salida',
        render: (item) => (item.fechaSalida ? new Date(item.fechaSalida).toLocaleString() : '—'),
      },
    ],
    [visitantes, anfitriones, departamentoPorAnfitrion]
  );

  const toolbar = (
    <label style={{ display: 'flex', alignItems: 'center', gap: 8, fontSize: 13, color: '#9b9ba8' }}>
      Estado:
      <select
        value={estado}
        onChange={(e) => setEstado(e.target.value)}
        style={{ padding: '5px 8px', color: '#f97316', background: 'var(--bg-input)', border: '1px solid var(--border)', borderRadius: 4 }}
      >
        <option value="">Todos</option>
        <option value="Pendiente">Pendiente</option>
        <option value="En Curso">En Curso</option>
        <option value="Finalizada">Finalizada</option>
      </select>
    </label>
  );

  const extraActions = (item) => (
    <>
      {item.estado === 'Pendiente' && (
        <button
          className="btn btn-primary"
          onClick={() => visitaService.registrarEntrada(item.id).then(() => setRefresh((n) => n + 1))}
        >
          Registrar entrada
        </button>
      )}
      {item.estado === 'En Curso' && (
        <button
          className="btn btn-secondary"
          onClick={() => visitaService.registrarSalida(item.id).then(() => setRefresh((n) => n + 1))}
        >
          Registrar salida
        </button>
      )}
    </>
  );

  return (
    <CrudPage
      title="Visitas (Control de Entrada/Salida)"
      service={service}
      fields={fields}
      columns={columns}
      toolbar={toolbar}
      extraActions={extraActions}
      refreshKey={refresh}
    />
  );
}
