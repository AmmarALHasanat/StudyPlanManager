<template>
  <div class="d-flex" id="wrapper">
    <!-- Sidebar -->
    <div class="border-end bg-dark text-white" id="sidebar-wrapper">
      <div class="sidebar-heading fw-bold text-center py-3 border-bottom">StudyPlan</div>
      <div class="list-group list-group-flush">
        <router-link
          to="/plans"
          class="list-group-item list-group-item-action bg-dark text-white border-0"
          active-class="active-link"
        >
          📚 Plans
        </router-link>
        <router-link
          to="/dashboard"
          class="list-group-item list-group-item-action bg-dark text-white border-0"
          active-class="active-link"
        >
          📈 Dashboard
        </router-link>
        <button
          class="list-group-item list-group-item-action bg-dark text-danger border-0 text-start"
          @click="logout"
        >
          🚪 Logout
        </button>
      </div>
    </div>

    <!-- Page Content -->
    <div id="page-content-wrapper" class="flex-grow-1">
      <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom px-4">
        <button class="btn btn-outline-dark btn-sm" @click="toggleSidebar">☰</button>
        <span class="ms-3 fw-bold">Study Plan Manager</span>
      </nav>

      <main class="container-fluid p-4">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const isSidebarOpen = ref(true);

function toggleSidebar() {
  const wrapper = document.getElementById("wrapper");
  if (wrapper) wrapper.classList.toggle("toggled");
}

function logout() {
  localStorage.removeItem("token");
  router.push("/login");
}
</script>

<style scoped>
#wrapper {
  display: flex;
  width: 100%;
  min-height: 100vh;
  overflow-x: hidden;
}

#sidebar-wrapper {
  width: 220px;
  transition: all 0.3s ease;
}

#wrapper.toggled #sidebar-wrapper {
  margin-left: -220px;
}

#page-content-wrapper {
  flex: 1;
  background-color: #f8f9fa;
}

.active-link {
  background-color: #495057 !important;
}
</style>
