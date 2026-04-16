//const baseUrl = import.meta.env.VITE_API_BASE_URL || 'https://localhost:44309/api'
const baseUrl = 'https://localhost:44309/api'

async function handleResponse(response) {
  if (!response.ok) {
    const text = await response.text()
    throw new Error(text || 'Erro na comunicação com o servidor')
  }
  return response.status === 204 ? null : response.json()
}

export async function getTasks(status, search) {
  const params = new URLSearchParams()
  if (search) params.append('search', search)
  else if (status) params.append('status', status)

  console.log('Procurando na url:', `${baseUrl}/tasks?${params.toString()}`)
  const response = await fetch(`${baseUrl}/tasks?${params.toString()}`)
  console.log('Response status:', response.status)
  return handleResponse(response)
}

export async function getTaskById(id) {
  const response = await fetch(`${baseUrl}/tasks/${id}`)
  return handleResponse(response)
}

export async function createTask(task) {
  const response = await fetch(`${baseUrl}/tasks`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(task)
  })
  return handleResponse(response)
}

export async function updateTask(id, task) {
  const response = await fetch(`${baseUrl}/tasks/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(task)
  })
  return handleResponse(response)
}

export async function concludeTask(id, status) {
  const response = await fetch(`${baseUrl}/tasks/concluir/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      status: status
    })
  })
  return handleResponse(response)
}

export async function deleteTask(id) {
  const response = await fetch(`${baseUrl}/tasks/${id}`, {
    method: 'DELETE'
  })
  return handleResponse(response)
}
