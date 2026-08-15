import CrudPage from '../components/CrudPage';
import * as departamentoService from '../api/departamentoService';

const fields = [
  { name: 'nombre', label: 'Nombre', required: true },
  { name: 'descripcion', label: 'Descripción', required: true },
  { name: 'ubicacion', label: 'Ubicación', required: true },
];

const columns = [
  { key: 'nombre', label: 'Nombre' },
  { key: 'descripcion', label: 'Descripción' },
  { key: 'ubicacion', label: 'Ubicación' },
];

export default function Departamentos() {
  return <CrudPage title="Departamentos" service={departamentoService} fields={fields} columns={columns} />;
}
