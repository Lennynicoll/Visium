import CrudPage from '../components/CrudPage';
import * as oficinaService from '../api/oficinaService';

const fields = [
  { name: 'nombre', label: 'Nombre', required: true },
  { name: 'ubicacion', label: 'Ubicación', required: true },
  { name: 'extension', label: 'Extensión' },
  { name: 'descripcion', label: 'Descripción' },
];

const columns = [
  { key: 'nombre', label: 'Nombre' },
  { key: 'ubicacion', label: 'Ubicación' },
  { key: 'extension', label: 'Extensión' },
  { key: 'descripcion', label: 'Descripción' },
];

export default function Oficinas() {
  return <CrudPage title="Oficinas" service={oficinaService} fields={fields} columns={columns} />;
}
