import CrudPage from '../components/CrudPage';
import * as visitanteService from '../api/visitanteService';

const fields = [
  { name: 'nombre', label: 'Nombre', required: true },
  { name: 'apellido', label: 'Apellido', required: true },
  { name: 'correo', label: 'Correo', type: 'email', required: true },
  { name: 'telefono', label: 'Teléfono' },
];

const columns = [
  { key: 'nombre', label: 'Nombre' },
  { key: 'apellido', label: 'Apellido' },
  { key: 'correo', label: 'Correo' },
  { key: 'telefono', label: 'Teléfono' },
];

export default function Visitantes() {
  return <CrudPage title="Visitantes" service={visitanteService} fields={fields} columns={columns} />;
}
