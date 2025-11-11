<template>
  <div>
    <h3 class="fw-bold mb-4">Tasks for Week {{ weekId }}</h3>
    <button class="btn btn-success mb-3" @click="addTask">+ Add Task</button>

    <div v-if="tasks.length === 0" class="text-muted">No tasks yet.</div>

    <ul class="list-group">
      <li
        v-for="task in tasks"
        :key="task.id"
        class="list-group-item d-flex justify-content-between align-items-center"
      >
        <div>
          <strong>{{ task.title }}</strong> — {{ task.day }}
          <p class="text-muted mb-0">{{ task.status }}</p>
        </div>
        <div>
          <button class="btn btn-outline-primary btn-sm me-2" @click="completeTask(task.id)">
            Mark Done
          </button>
          <button class="btn btn-outline-danger btn-sm" @click="deleteTask(task.id)">
            Delete
          </button>
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import api from "../services/APIs/api";

interface Task {
  id: number;
  title: string;
  day: string;
  status: string;
}

const route = useRoute();
const weekId = route.params.id;
const tasks = ref<Task[]>([]);

async function fetchTasks() {
  const res = await api.get(`/task/week/${weekId}`);
  tasks.value = res.data;
}

async function addTask() {
  const title = prompt("Enter task title:");
  if (!title) return;
  await api.post(`/task/${weekId}`, {
    day: "Monday",
    title,
    activityType: "Reading",
    notes: "",
    status: "Pending",
  });
  fetchTasks();
}

async function deleteTask(id: number) {
  await api.delete(`/task/${id}`);
  fetchTasks();
}

async function completeTask(id: number) {
  await api.put(`/task/${id}`, { status: "Done" });
  fetchTasks();
}

onMounted(fetchTasks);
</script>
