<script setup>
import { ref, onMounted } from 'vue'
import TaskForm from './components/TaskForm.vue'
import TaskList from './components/TaskList.vue'
import { getTasks, createTask, updateTask, deleteTask, concludeTask} from './services/taskService'

const tasks = ref([])
const loading = ref(false)
const error = ref('')
const statusFilter = ref('')
const searchTerm = ref('')
const selectedTask = ref(null)

async function loadTasks() {
  loading.value = true
  error.value = ''
  try {
    tasks.value = await getTasks(statusFilter.value, searchTerm.value)
  } catch (err) {
    error.value = err instanceof Error ? err.message : String(err)
  } finally {
    loading.value = false
  }
}

async function handleSubmit(payload) {
  loading.value = true
  error.value = ''

  try {
    if (selectedTask.value) {
      await updateTask(selectedTask.value.id, payload)
      selectedTask.value = null
    } else {
      await createTask(payload)
    }
    await loadTasks()
  } catch (err) {
    error.value = err instanceof Error ? err.message : String(err)
  } finally {
    loading.value = false
  }
}

async function handleConclude(task) {
  loading.value = true
  error.value = ''

  try {
    const newStatus = task.status === 2 ? 0 : 2// alterna entre Concluído e Pendente

    await concludeTask(task.id, newStatus)

    await loadTasks()
  } catch (err) {
    error.value = err instanceof Error ? err.message : String(err)
  } finally {
    loading.value = false
  }
}

function handleEdit(task) {
  selectedTask.value = { ...task }
}

async function handleDelete(id) {
  const confirmed = window.confirm('Tem certeza que deseja excluir esta tarefa?')
  if (!confirmed) return

  loading.value = true
  error.value = ''
  try {
    await deleteTask(id)
    if (selectedTask.value?.id === id) {
      selectedTask.value = null
    }
    await loadTasks()
  } catch (err) {
    error.value = err instanceof Error ? err.message : String(err)
  } finally {
    loading.value = false
  }
}

function handleCancel() {
  selectedTask.value = null
}

function handleStatusChange(value) {
  statusFilter.value = value
  searchTerm.value = ''
  loadTasks()
}

function handleSearchChange(value) {
  searchTerm.value = value
  statusFilter.value = ''
  loadTasks()
}

onMounted(loadTasks)
</script>

<template>
  <main class="app-shell">
    <header class="app-header">
      <div>
        <h1>Gestão de Tarefas</h1>
      </div>
    </header>

    <div class="content-grid">
      <TaskForm
        :modelValue="selectedTask"
        submitLabel="Salvar tarefa"
        :loading="loading"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />

      <TaskList
        :tasks="tasks"
        :loading="loading"
        :error="error"
        :statusFilter="statusFilter"
        :searchTerm="searchTerm"
        @change-status="handleStatusChange"
        @change-search="handleSearchChange"
        @edit="handleEdit"
        @delete="handleDelete"
        @refresh="loadTasks"
        @conclude="handleConclude"
      />
    </div>
  </main>
</template>
