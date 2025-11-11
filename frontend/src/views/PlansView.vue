<template>
  <div class="container">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h3 class="fw-bold">Your Plans</h3>
      <button class="btn btn-success" @click="addPlan">+ Add Plan</button>
    </div>

    <div v-if="plans.length === 0" class="text-muted">No plans yet.</div>

    <div class="row row-cols-1 row-cols-md-2 g-4">
      <div v-for="plan in plans" :key="plan.id" class="col">
        <div class="card shadow-sm border-0">
          <div class="card-body">
            <h5 class="card-title fw-bold">{{ plan.name }}</h5>
            <p class="card-text text-muted">{{ plan.description }}</p>
            <button class="btn btn-outline-primary btn-sm me-2" @click="openWeeks(plan.id)">
              Open
            </button>
            <button class="btn btn-outline-danger btn-sm" @click="deletePlan(plan.id)">
              Delete
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import api from "../services/APIs/api";

interface Plan {
  id: number;
  name: string;
  description: string;
}

const router = useRouter();
const plans = ref<Plan[]>([]);

async function fetchPlans() {
  const res = await api.get("/plan");
  plans.value = res.data;
}

async function addPlan() {
  const name = prompt("Enter plan name:");
  if (!name) return;
  await api.post("/plan", {
    name,
    durationWeeks: 8,
    description: "New plan",
    startDate: new Date(),
    endDate: new Date(),
  });
  fetchPlans();
}

async function deletePlan(id: number) {
  if (!confirm("Are you sure?")) return;
  await api.delete(`/plan/${id}`);
  fetchPlans();
}

function openWeeks(id: number) {
  router.push(`/weeks/${id}`);
}

onMounted(fetchPlans);
</script>
