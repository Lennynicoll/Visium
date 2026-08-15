import { useState, useEffect, useMemo } from 'react';
import CrudPage from '../components/CrudPage';
import * as anfitrionService from '../api/anfitrionService';
import * as departamentoService from '../api/departamentoService';
import * as motivoService from '../api/motivoService';
import { formatCedula, maskCedula } from '../utils/format';

const fields = [
  { name: 'nombre', label: 'Nombre', required: true },
  { name: 'apellido', label: 'Apellido', required: true },
  { name: 'cedula', label: 'Cédula', required: true, placeholder: '000-0000000-0', format: maskCedula },
  { name: 'telefono', label: 'Teléfono', required: true },
  { name: 'correo', label: 'Correo', type: 'email' },
  { name: 'horarioAtencion', label: 'Horario de atención:', inline: true, width: 200 },
  {
    name: 'departamentoId',
    label: 'Departamento',
    type: 'select',
    required: true,
    loadOptions: () =>
      departamentoService.getAll().then((res) =>
        res.data.map((d) => ({ value: d.id, text: d.nombre }))
      ),
  },
  {
    name: 'motivoVisitaId',
    label: 'Tipo de atención',
    type: 'select',
    required: true,
    loadOptions: () =>
      motivoService.getAll().then((res) =>
        res.data.map((m) => ({ value: m.id, text: m.nombre }))
      ),
  },
];

export default function Anfitriones() {
  const [departamentos, setDepartamentos] = useState({});
  const [motivos, setMotivos] = useState({});

  useEffect(() => {
    departamentoService
      .getAll()
      .then((res) => setDepartamentos(Object.fromEntries(res.data.map((d) => [d.id, d.nombre]))))
      .catch(() => {});
    motivoService
      .getAll()
      .then((res) => setMotivos(Object.fromEntries(res.data.map((m) => [m.id, m.nombre]))))
      .catch(() => {});
  }, []);

  const columns = useMemo(
    () => [
      { key: 'nombre', label: 'Nombre' },
      { key: 'apellido', label: 'Apellido' },
      { key: 'cedula', label: 'Cédula', render: (item) => formatCedula(item.cedula) },
      { key: 'correo', label: 'Correo' },
      { key: 'horarioAtencion', label: 'Horario' },
      {
        key: 'departamentoId',
        label: 'Departamento',
        render: (item) => departamentos[item.departamentoId] || `Depto. #${item.departamentoId}`,
      },
      {
        key: 'motivoVisitaId',
        label: 'Tipo de atención',
        render: (item) => motivos[item.motivoVisitaId] || `Motivo #${item.motivoVisitaId}`,
      },
    ],
    [departamentos, motivos]
  );

  return <CrudPage title="Anfitriones" service={anfitrionService} fields={fields} columns={columns} />;
}
