<script setup>
import { ref } from 'vue'

const toasts = ref([])

let id = 0

function show(message, type = 'success') {
  const toast = { id: id++, message, type }
  toasts.value.push(toast)

  setTimeout(() => {
    remove(toast.id)
  }, 3000)
}

function remove(id) {
  toasts.value = toasts.value.filter(t => t.id !== id)
}

defineExpose({ show })
</script>

<style scoped>
.toast-container {
  position: fixed;
  bottom: 20px;     
  left: 50%;
  transform: translateX(-50%); 
  width: 50%;

  display: flex;
  flex-direction: column;
  gap: 10px;
  z-index: 999;
}

.toast {
  padding: 12px 16px;
  border-radius: 8px;
  min-width: 200px;
  animation: fadeIn 0.4s ease;
}

.toast.success {
  background: #ecfdf5;     
  color: #065f46;   
  border: 1px solid #10b981;
}

.toast.error {
  background: #fef2f2; 
  color: #7f1d1d;
  border: 1px solid #ef4444;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>

<template>
  <div class="toast-container">
    <div 
      v-for="toast in toasts" 
      :key="toast.id" 
      :class="['toast', toast.type]"
    >
      {{ toast.message }}
    </div>
  </div>
</template>