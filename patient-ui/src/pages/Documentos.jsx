import { useState, useEffect, useMemo } from 'react';
import CrudPage from '../components/CrudPage';
import * as documentoService from '../api/documentoService';
import * as visitanteService from '../api/visitanteService';
import { maskCedula } from '../utils/format';

const fields = [
  {
    name: 'tipo',
    label: 'Tipo de documento',
    type: 'select',
    required: true,
    loadOptions: async () => [
      { value: 'Cédula', text: 'Cédula' },
      { value: 'Pasaporte', text: 'Pasaporte' },
      { value: 'Licencia', text: 'Licencia de conducir' },
      { value: 'Carnet', text: 'Carnet institucional' },
      { value: 'Otro', text: 'Otro' },
    ],
  },
  {
    name: 'numero',
    label: 'Número',
    required: true,
    placeholder: '000-0000000-0',
    format: (value, form) => (form.tipo === 'Cédula' ? maskCedula(value) : value),
  },
  { name: 'fechaExpedicion', label: 'Fecha expedición', type: 'date', required: true },
  { name: 'fechaVencimiento', label: 'Fecha vencimiento', type: 'date', required: true },
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
];

function estadoDocumento(fechaVencimiento) {
  if (!fechaVencimiento) return { text: '', cls: '' };
  const hoy = new Date();
  hoy.setHours(0, 0, 0, 0);
  const vence = new Date(fechaVencimiento);
  vence.setHours(0, 0, 0, 0);
  if (vence < hoy) return { text: 'Vencido', cls: 'status-vencido' };
  const limite = new Date(hoy);
  limite.setDate(limite.getDate() + 30);
  if (vence <= limite) return { text: 'Vence pronto', cls: 'status-por-vencer' };
  return { text: 'Vigente', cls: 'status-vigente' };
}

export default function Documentos() {
  const [visitantes, setVisitantes] = useState({});

  useEffect(() => {
    visitanteService
      .getAll()
      .then((res) => setVisitantes(Object.fromEntries(res.data.map((v) => [v.id, `${v.nombre} ${v.apellido}`]))))
      .catch(() => {});
  }, []);

  const columns = useMemo(
    () => [
      { key: 'tipo', label: 'Tipo' },
      { key: 'numero', label: 'Número' },
      {
        key: 'fechaExpedicion',
        label: 'Expedición',
        render: (item) => (item.fechaExpedicion ? item.fechaExpedicion.split('T')[0] : ''),
      },
      {
        key: 'fechaVencimiento',
        label: 'Vencimiento',
        render: (item) => (item.fechaVencimiento ? item.fechaVencimiento.split('T')[0] : ''),
      },
      {
        key: 'estado',
        label: 'Estado',
        render: (item) => {
          const s = estadoDocumento(item.fechaVencimiento);
          return s.text ? <span className={`status ${s.cls}`}>{s.text}</span> : '';
        },
      },
      {
        key: 'visitanteId',
        label: 'Visitante',
        render: (item) => visitantes[item.visitanteId] || `Visitante #${item.visitanteId}`,
      },
    ],
    [visitantes]
  );

  return (
    <CrudPage
      title="Documentos de Visitantes"
      service={documentoService}
      fields={fields}
      columns={columns}
      toolbar={
        <span style={{ fontSize: 13, color: '#9b9ba8' }}>
          Documento de identificación del visitante (cédula, pasaporte, etc.) con su vigencia
        </span>
      }
    />
  );
}
