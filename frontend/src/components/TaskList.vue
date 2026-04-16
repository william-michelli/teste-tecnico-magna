<script setup>
const props = defineProps({
  tasks: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  error: { type: String, default: '' },
  statusFilter: { type: [String, Number], default: '' },
  searchTerm: { type: String, default: '' }
})
const emit = defineEmits(['change-status', 'change-search', 'edit', 'delete', 'refresh'])

const statusOptions = [
  { label: 'Todas', value: '' },
  { label: 'Pendente', value: 'Pendente' },
  { label: 'Em andamento', value: 'EmAndamento' },
  { label: 'Concluído', value: 'Concluido' }
]

function onStatusChange(event) {
  emit('change-status', event.target.value)
}

function onSearchChange(event) {
  emit('change-search', event.target.value)
}
</script>

<template>
  <section class="task-list-card">
    <div class="toolbar">
      <div class="filters">
        <label>
          Filtrar por status
          <select :value="statusFilter" @change="onStatusChange">
            <option v-for="option in statusOptions" :key="option.value" :value="option.value">
              {{ option.label }}
            </option>
          </select>
        </label>
        <label>
          Buscar
          <input
            type="search"
            :value="searchTerm"
            @input="onSearchChange"
            placeholder="Título ou descrição"
          />
        </label>
      </div>

      <button class="secondary" @click="$emit('refresh')" :disabled="loading">
        Atualizar
      </button>
    </div>

    <div class="status-row">
      <span v-if="loading">Carregando tarefas...</span>
      <span v-else-if="error" class="error">{{ error }}</span>
      <span v-else-if="tasks.length === 0">Nenhuma tarefa encontrada.</span>
      <span v-else>{{ tasks.length }} tarefa(s) encontradas.</span>
    </div>

    <table class="task-table" v-if="tasks.length > 0">
      <thead>
        <tr>
          <th>Título</th>
          <th>Descrição</th>
          <th>Status</th>
          <th>Criado em</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="task in tasks" :key="task.id">
          <td>{{ task.title }}</td>
          <td>{{ task.description }}</td>
          <td>{{ task.status }}</td>
          <td>{{ new Date(task.createdAt).toLocaleString() }}</td>
          <td class="actions">
            <button @click="$emit('edit', task)">Editar</button>
            <button class="danger" @click="$emit('delete', task.id)">Excluir</button>
          </td>
        </tr>
      </tbody>
    </table>
  </section>
</template>
