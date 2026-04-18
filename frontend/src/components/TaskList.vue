<script setup>
import { PencilIcon, TrashIcon, ArrowPathIcon } from '@heroicons/vue/24/solid'

const props = defineProps({
  tasks: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  error: { type: String, default: '' },
  statusFilter: { type: [String, Number], default: '' },
  searchTerm: { type: String, default: '' }
})
const emit = defineEmits(['change-status', 'change-search', 'edit', 'delete', 'refresh', 'conclude'])

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

function getStatusText(status) {
  switch (status) {
    case 0: return 'Pendente'
    case 1: return 'Em Andamento'
    case 2: return 'Concluído'
    default: return ''
  }
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

      <button class="action-button" @click="$emit('refresh')" :disabled="loading">
        <ArrowPathIcon class="icon icon-refresh"/>
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
          <th></th>
          <th>Título</th>
          <th>Descrição</th>
          <th>Status</th>
          <th>Criado em</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="task in tasks" :key="task.id">
          <td>
            <label class="custom-checkbox">
              <input 
                type="checkbox"
                :checked="task.status === 2"
                @change="$emit('conclude', task)"
              />
              <span class="checkmark"></span>
            </label>
          </td>
          <td>{{ task.title }}</td>
          <td>{{ task.description }}</td>
          <td>{{ getStatusText(task.status) }}</td>
          <td>{{ new Date(task.createdAt).toLocaleString('pt-BR', {
              day: '2-digit',
              month: '2-digit',
              year: 'numeric'
            }) }}
          </td>
          <td class="actions">
            <button class="action-button" @click="$emit('edit', task)">     
              <PencilIcon class="icon" />
            </button>
            <button class="action-button" @click="$emit('delete', task.id)">
              <TrashIcon class="icon" />
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </section>
</template>
