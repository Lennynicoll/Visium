import { useState, useEffect, useCallback } from 'react';

function emptyForm(fields) {
  const f = {};
  fields.forEach((field) => {
    f[field.name] = field.defaultValue ?? '';
  });
  return f;
}

export default function CrudPage({
  title,
  service,
  fields,
  columns,
  extraActions,
  toolbar,
  idLabel = 'ID',
  refreshKey = 0,
}) {
  const [items, setItems] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [editing, setEditing] = useState(null);
  const [error, setError] = useState('');
  const [form, setForm] = useState({});
  const [optionMap, setOptionMap] = useState({});

  const fetchData = useCallback(() => {
    service
      .getAll()
      .then((res) => setItems(res.data))
      .catch((err) => setError(err.message));
  }, [service]);

  useEffect(() => {
    fetchData();
  }, [fetchData, refreshKey]);

  useEffect(() => {
    const interval = setInterval(fetchData, 5000);
    return () => clearInterval(interval);
  }, [fetchData]);

  useEffect(() => {
    if (!showForm) return;
    const load = async () => {
      const map = {};
      for (const field of fields) {
        if (field.type === 'select' && field.loadOptions) {
          try {
            map[field.name] = await field.loadOptions(form);
          } catch {
            map[field.name] = [];
          }
        }
      }
      setOptionMap(map);
    };
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [showForm, fields]);

  const startCreate = () => {
    setEditing(null);
    setForm(emptyForm(fields));
    setError('');
    setShowForm(true);
  };

  const startEdit = (item) => {
    setEditing(item);
    setForm({ ...emptyForm(fields), ...item });
    setError('');
    setShowForm(true);
  };

  const handleChange = (e) => {
    const field = fields.find((f) => f.name === e.target.name);
    const value = field?.format ? field.format(e.target.value, form) : e.target.value;
    const nextForm = { ...form, [e.target.name]: value };
    setForm(nextForm);
    fields.forEach(async (f) => {
      if (f.type === 'select' && f.dependeDe === e.target.name && f.loadOptions) {
        try {
          const opts = await f.loadOptions(nextForm);
          setOptionMap((prev) => ({ ...prev, [f.name]: opts }));
        } catch {
          setOptionMap((prev) => ({ ...prev, [f.name]: [] }));
        }
        if (nextForm[f.name]) {
          setForm((prev) => ({ ...prev, [f.name]: '' }));
        }
      }
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const action = editing ? service.update(editing.id, form) : service.create(form);
    action
      .then(() => {
        setShowForm(false);
        setEditing(null);
        fetchData();
      })
      .catch((err) => {
        const detail = err.response?.data?.errors;
        let msg;
        if (Array.isArray(detail)) {
          msg = detail.join(', ');
        } else if (detail && typeof detail === 'object') {
          msg = Object.values(detail).flat().join(', ');
        } else {
          msg = err.message;
        }
        setError(msg);
      });
  };

  const handleDelete = (id) => {
    if (!window.confirm(`Eliminar el registro #${id}?`)) return;
    service
      .remove(id)
      .then(() => fetchData())
      .catch((err) => setError(err.message));
  };

  const renderField = (field) => {
    if (field.type === 'select') {
      const options = optionMap[field.name] || [];
      return (
        <label key={field.name}>
          {field.label}
          <select name={field.name} value={form[field.name] ?? ''} onChange={handleChange}>
            <option value="">Seleccione...</option>
            {options.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.text}
              </option>
            ))}
          </select>
        </label>
      );
    }
    return (
      <label key={field.name} className={field.inline ? 'field-inline' : undefined}>
        {field.label}
        <input
          name={field.name}
          type={field.type || 'text'}
          value={form[field.name] ?? ''}
          placeholder={field.placeholder}
          style={field.width ? { width: field.width } : undefined}
          onChange={handleChange}
          {...(field.required ? { required: true } : {})}
        />
      </label>
    );
  };

  return (
    <div>
      <div className="page-header">
        <div>
          <h1>{title}</h1>
          {toolbar}
        </div>
        <button className="btn btn-primary" onClick={() => (showForm ? setShowForm(false) : startCreate())}>
          {showForm ? 'Cerrar' : '+ Nuevo'}
        </button>
      </div>
      {error && <div className="error">{error}</div>}
      {showForm && (
        <form className="form-row" onSubmit={handleSubmit}>
          {fields.map(renderField)}
          <button type="submit" className="btn btn-primary">
            {editing ? 'Actualizar' : 'Guardar'}
          </button>
          <button type="button" className="btn btn-secondary" onClick={() => setShowForm(false)}>
            Cancelar
          </button>
        </form>
      )}
      <div className="table-wrap">
        <table>
        <thead>
          <tr>
            <th>{idLabel}</th>
            {columns.map((col) => (
              <th key={col.key}>{col.label}</th>
            ))}
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {items.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              {columns.map((col) => (
                <td key={col.key}>{col.render ? col.render(item) : item[col.key]}</td>
              ))}
              <td>
                <div className="actions">
                  <div className="actions-row">
                    <button className="btn btn-edit" onClick={() => startEdit(item)}>
                      Editar
                    </button>
                    <button className="btn btn-delete" onClick={() => handleDelete(item.id)}>
                      Eliminar
                    </button>
                  </div>
                  {extraActions && <div className="actions-row">{extraActions(item)}</div>}
                </div>
              </td>
            </tr>
          ))}
          {items.length === 0 && (
            <tr>
              <td colSpan={columns.length + 2} className="empty">
                No hay registros
              </td>
            </tr>
          )}
        </tbody>
      </table>
      </div>
    </div>
  );
}
