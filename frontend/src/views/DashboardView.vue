<template>
  <div>
    <h3 class="fw-bold mb-4">Dashboard</h3>

    <div class="row">
      <div class="col-md-4 mb-3">
        <div class="card shadow-sm border-0 p-3 text-center">
          <h5>Total Plans</h5>
          <h2>{{ stats.plans }}</h2>
        </div>
      </div>
      <div class="col-md-4 mb-3">
        <div class="card shadow-sm border-0 p-3 text-center">
          <h5>Total Weeks</h5>
          <h2>{{ stats.weeks }}</h2>
        </div>
      </div>
      <div class="col-md-4 mb-3">
        <div class="card shadow-sm border-0 p-3 text-center">
          <h5>Total Tasks</h5>
          <h2>{{ stats.tasks }}</h2>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import api from "../services/APIs/api";

const stats = ref({ plans: 0, weeks: 0, tasks: 0 });

onMounted(async () => {
  const plans = await api.get("/plan");
  const weeks = plans.data.flatMap((p: any) => p.weeks || []);
  const tasks = weeks.flatMap((w: any) => w.tasks || []);
  stats.value = {
    plans: plans.data.length,
    weeks: weeks.length,
    tasks: tasks.length,
  };
});
</script>
