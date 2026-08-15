import CrudPage from '../components/CrudPage';
import * as motivoService from '../api/motivoService';

const fields = [
  { name: 'nombre', label: 'Nombre', required: true },
  { name: 'descripcion', label: 'Descripción', required: true },
];

const columns = [
  { key: 'nombre', label: 'Nombre' },
  { key: 'descripcion', label: 'Descripción' },
];

export default function MotivosVisita() {
  return <CrudPage title="Motivos de Visita" service={motivoService} fields={fields} columns={columns} />;
}
