<script setup>
import { PencilIcon, TrashIcon, ArrowPathIcon, ChevronLeftIcon, ChevronRightIcon, PlusSmallIcon  } from '@heroicons/vue/24/solid'

const props = defineProps({
  pagedTasks: { type: Object, default: () => ({ items: [] }) },
  loading: { type: Boolean, default: false },
  error: { type: String, default: '' },
  statusFilter: { type: [String, Number], default: '' },
  searchTerm: { type: String, default: '' }
})
const emit = defineEmits(['change-status', 'change-search', 'edit', 'delete', 'refresh', 'conclude', 'change-page'])

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

<style scoped>
/* ############## Checkmark ############## */

.task-table th {
  color: #5f6c84;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  font-size: 0.78rem;
  text-align: center;
}

.custom-checkbox {
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}

.custom-checkbox input {
  display: none;
}

/* Caixa */
.checkmark {
  width: 20px;
  height: 20px;
  border: 2px solid #666;
  border-radius: 8px;
  position: relative;
  transition: all 0.2s ease;
}

.custom-checkbox:hover .checkmark {
  border-color: #4f7cff;
}

.custom-checkbox input:checked + .checkmark {
  background-color: #4f7cff;
  border-color: #4f7cff;
  transform: scale(1.0);
}

.checkmark::after {
  content: "";
  position: absolute;
  display: none;
}

.custom-checkbox input:checked + .checkmark::after {
  display: block;
}

.checkmark::after {
  left: 4px;
  top: 0px;
  width: 7px;
  height: 10px;
  border: solid white;
  border-width: 0 3px 3px 0;
  transform: rotate(45deg);
}

/* ####################################### */
/* ############## Paginação ############## */

.pagination {
  display: flex;
  gap: 10px;
  justify-content: center;
  margin-top: 20px;
}

.pagina-atual {
  align-self: center;
}

/* ############################################# */
/* ############## Botões de Ações ############## */

button.action-button {
  border: none;
  cursor: pointer;
  padding: 3px 3px;
  background: none;
}

.actions {
  display: flex;
  flex-direction: column;
  gap: 2px;
  text-align: center;
}

.icon {
  width: 20px;
  height: 20px;
  color: rgb(57, 57, 61); /* importante */
}

.icon-refresh {
  color: black; /* importante */
}

/* ############################################# */
/* ################## Tabela ################### */

.toolbar {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  gap: 16px;
  margin-bottom: 18px;
}

.filters {
  display: flex;
  flex-wrap: wrap;
  gap: 16px;
}

.status-row {
  margin-bottom: 14px;
  font-size: 0.95rem;
}

.status-row .error {
  color: #bf1616;
}

.task-table {
  width: 100%;
  border-collapse: collapse;
}

.task-table th,
.task-table td {
  padding: 4px 0px;
  text-align: center;
  border-bottom: 1px solid #eef2ff;
}

.task-table td {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

table {
  width: 100%;
  table-layout: fixed; 
}

.task-done td {
  text-decoration: line-through;
  color: #999;
}

.task-done {
  background-color: #f5f5f5de;
}

</style>

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
      <span v-else-if="pagedTasks.items.length === 0">Nenhuma tarefa encontrada.</span>
      <span v-else>{{ pagedTasks.items.length ?? 0}} tarefa(s) encontradas.</span>
    </div>

    <table class="task-table" v-if="!loading && pagedTasks.items.length > 0">
      <colgroup>
        <col style="width: 5%;">
        <col style="width: 20%;">
        <col style="width: 31%;">
        <col style="width: 15%;">
        <col style="width: 12%;">
        <col style="width: 12%;">
        <col style="width: 5%;">
      </colgroup>

      <thead>
        <tr>
          <th></th>
          <th>Título</th>
          <th>Descrição</th>
          <th>Status</th>
          <th>Criado em</th>
          <th>Editado em</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr 
          v-for="task in pagedTasks.items" 
          :key="task.id"
          :class="{ 'task-done': task.status === 2 }"
        >
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
          <td>{{ task.description || '-' }}</td>
          <td>{{ getStatusText(task.status) }}</td>
          <td>{{ new Date(task.createdAt).toLocaleString('pt-BR', {
              day: '2-digit',
              month: '2-digit',
              year: '2-digit'
            }) }}
          </td>
          <td>{{ new Date(task.editedAt).toLocaleString('pt-BR', {
              day: '2-digit',
              month: '2-digit',
              year: '2-digit'
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

    <div class="pagination">
      <!-- Voltar -->
      <button 
        @click="$emit('change-page', pagedTasks.page - 1)" 
        :style="{ visibility: pagedTasks.page === 1 ? 'hidden' : 'visible' }"
      >
        <ChevronLeftIcon class="icon" />
      </button>

      <span class="pagina-atual">Página <strong>{{ pagedTasks.page }}</strong> de <strong>{{ pagedTasks.totalPages }}</strong> </span>

      <!-- Avançar -->
      <button 
        @click="$emit('change-page', pagedTasks.page + 1)" 
       :style="{ visibility: pagedTasks.page >= pagedTasks.totalPages ? 'hidden' : 'visible' }"
      >
          <ChevronRightIcon class="icon" />
      </button>
    </div>
  </section>
</template>
