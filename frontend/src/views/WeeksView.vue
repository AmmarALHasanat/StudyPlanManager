<template>
  <div>
    <h3 class="fw-bold mb-4">Weeks of Plan {{ planId }}</h3>
    <button class="btn btn-success mb-3" @click="addWeek">+ Add Week</button>

    <div v-if="weeks.length === 0" class="text-muted">No weeks yet.</div>

    <ul class="list-group">
      <li
        v-for="week in weeks"
        :key="week.id"
        class="list-group-item d-flex justify-content-between align-items-center"
      >
        <div>
          <strong>Week {{ week.weekNumber }}</strong> — {{ week.summary }}
        </div>
        <div>
          <button class="btn btn-outline-primary btn-sm me-2" @click="openTasks(week.id)">
            Tasks
          </button>
          <button class="btn btn-outline-danger btn-sm" @click="deleteWeek(week.id)">
            Delete
          </button>
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import api from "../services/APIs/api";

interface Week {
  id: number;
  weekNumber: number;
  summary: string;
}

const route = useRoute();
const router = useRouter();
const planId = route.params.id;
const weeks = ref<Week[]>([]);

async function fetchWeeks() {
  const res = await api.get(`/week/${planId}`);
  weeks.value = res.data;
}

async function addWeek() {
  await api.post(`/week/${planId}`, {
    weekNumber: weeks.value.length + 1,
    summary: "New Week",
    progress: 0,
  });
  fetchWeeks();
}

async function deleteWeek(id: number) {
  await api.delete(`/week/${id}`);
  fetchWeeks();
}

function openTasks(id: number) {
  router.push(`/tasks/${id}`);
}

onMounted(fetchWeeks);
</script>
