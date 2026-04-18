<script setup>
import { ref, watch, computed } from 'vue'

const props = defineProps({
  modelValue: {
    type: Object,
    default: null
  },
  submitLabel: {
    type: String,
    default: 'Salvar'
  },
  loading: {
    type: Boolean,
    default: false
  }
})
const emit = defineEmits(['submit', 'cancel'])

const statuses = [
  { label: 'Pendente', value: 0 },
  { label: 'Em andamento', value: 1 },
  { label: 'Concluído', value: 2 }
]

const form = ref({
  title: '',
  description: '',
  status: 0
})

watch(
  () => props.modelValue,
  (task) => {
    if (task) {
      form.value = {
        title: task.title || '',
        description: task.description || '',
        status: task.status ?? 0
      }
    } else {
      form.value = { title: '', description: '', status: 0 }
    }
  },
  { immediate: true }
)

const heading = computed(() => props.modelValue ? 'Editar tarefa' : 'Nova tarefa')

function onSubmit() {
  emit('submit', { ...form.value })
}

function onCancel() {
  emit('cancel')
}
</script>

<template>
  <section class="task-form-card">
    <header>
      <h2>{{ heading }}</h2>
    </header>

    <div class="form-grid">
      <label>
        Título
        <input
          type="text"
          v-model="form.title"
          placeholder="Digite o título da tarefa"
          :disabled="loading"
        />
      </label>

      <label>
        Descrição
        <textarea
          v-model="form.description"
          placeholder="Digite a descrição"
          rows="4"
          :disabled="loading"
        />
      </label>

      <label>
        Status
        <select v-model.number="form.status" :disabled="loading">
          <option v-for="option in statuses" :key="option.value" :value="option.value">
            {{ option.label }}
          </option>
        </select>
      </label>
    </div>

    <div class="form-actions">
      <button class="primary" @click="onSubmit" :disabled="loading || !form.title.trim()">
        {{ submitLabel }}
      </button>
      <button class="secondary" @click="onCancel" type="button" :disabled="loading">
        Cancelar
      </button>
    </div>
  </section>
</template>


