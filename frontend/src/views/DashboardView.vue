<template>
  <div class="dashboard-container">
    <div class="dashboard-header">
      <h1>🚗 Dashboard - Sistema de Parking</h1>
      <div class="user-info">
        <span>Bienvenido, {{ authStore.usuario?.correo }}</span>
        <button @click="handleLogout" class="logout-button">
          Cerrar Sesión
        </button>
      </div>
    </div>

    <!-- Estadísticas rápidas -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon">🅿️</div>
        <div class="stat-content">
          <h3>{{ plazasStore.plazas.length }}</h3>
          <p>Total Plazas</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">✅</div>
        <div class="stat-content">
          <h3>{{ plazasStore.plazasDisponibles.length }}</h3>
          <p>Plazas Disponibles</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">📅</div>
        <div class="stat-content">
          <h3>{{ reservasStore.reservasHoy.length }}</h3>
          <p>Reservas Hoy</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">💰</div>
        <div class="stat-content">
          <h3>{{ totalGanado }}€</h3>
          <p>Total Ganado</p>
        </div>
      </div>
    </div>

    <!-- Filtros de búsqueda -->
    <div class="filters-section">
      <h2>Filtrar Plazas</h2>
      <div class="filters-grid">
        <div class="filter-group">
          <label for="planta">Planta:</label>
          <select id="planta" v-model="filters.planta">
            <option value="">Todas</option>
            <option value="0">Planta 0</option>
            <option value="1">Planta 1</option>
            <option value="2">Planta 2</option>
          </select>
        </div>
        
        <div class="filter-group">
          <label for="zona">Zona:</label>
          <select id="zona" v-model="filters.zona">
            <option value="">Todas</option>
            <option value="A">Zona A</option>
            <option value="B">Zona B</option>
            <option value="C">Zona C</option>
          </select>
        </div>
        
        <div class="filter-group">
          <label for="tipo">Tipo:</label>
          <select id="tipo" v-model="filters.tipo">
            <option value="">Todos</option>
            <option value="Coche">Coche</option>
            <option value="Moto">Moto</option>
            <option value="Grande">Grande</option>
          </select>
        </div>
        
        <div class="filter-group">
          <label for="disponible">Disponibilidad:</label>
          <select id="disponible" v-model="filters.disponible">
            <option value="">Todas</option>
            <option value="true">Disponibles</option>
            <option value="false">Ocupadas</option>
          </select>
        </div>
        
        <div class="filter-group">
          <label for="precio-min">Precio Mín:</label>
          <input 
            id="precio-min" 
            v-model="filters.precioMin" 
            type="number" 
            min="0" 
            step="0.5"
            placeholder="0.00"
          />
        </div>
        
        <div class="filter-group">
          <label for="precio-max">Precio Máx:</label>
          <input 
            id="precio-max" 
            v-model="filters.precioMax" 
            type="number" 
            min="0" 
            step="0.5"
            placeholder="10.00"
          />
        </div>
      </div>
      
      <div class="filter-actions">
        <button @click="applyFilters" class="apply-filters-button">
          Aplicar Filtros
        </button>
        <button @click="clearFilters" class="clear-filters-button">
          Limpiar Filtros
        </button>
      </div>
    </div>

    <!-- Lista de plazas -->
    <div class="plazas-section">
      <div class="section-header">
        <h2>Plazas de Parking</h2>
        <button @click="refreshPlazas" class="refresh-button">
          🔄 Actualizar
        </button>
      </div>

      <div v-if="plazasStore.isLoading" class="loading">
        Cargando plazas...
      </div>

      <div v-else-if="plazasStore.error" class="error-message">
        {{ plazasStore.error }}
      </div>

      <div v-else-if="filteredPlazas.length === 0" class="no-results">
        No se encontraron plazas con los filtros aplicados
      </div>

      <div v-else class="plazas-grid">
        <div 
          v-for="plaza in filteredPlazas" 
          :key="plaza.id"
          class="plaza-card"
          :class="{ 'ocupada': !plaza.disponible }"
        >
          <div class="plaza-header">
            <h3>Plaza {{ plaza.numero }}</h3>
            <span class="plaza-status" :class="{ 'disponible': plaza.disponible, 'ocupada': !plaza.disponible }">
              {{ plaza.disponible ? 'Disponible' : 'Ocupada' }}
            </span>
          </div>
          
          <div class="plaza-details">
            <p><strong>Planta:</strong> {{ plaza.planta }}</p>
            <p><strong>Zona:</strong> {{ plaza.zona }}</p>
            <p><strong>Tipo:</strong> {{ plaza.tipo }}</p>
            <p><strong>Precio:</strong> {{ plaza.precioHora }}€/hora</p>
          </div>
          
          <div class="plaza-actions">
            <button 
              v-if="plaza.disponible"
              @click="reservarPlaza(plaza)"
              class="reservar-button"
            >
              🚗 Reservar
            </button>
            <button 
              v-else
              @click="verDetallesPlaza(plaza)"
              class="detalles-button"
            >
              📋 Ver Detalles
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal de reserva -->
    <div v-if="showReservaModal" class="modal-overlay" @click="showReservaModal = false">
      <div class="modal" @click.stop>
        <h3>Reservar Plaza {{ selectedPlaza?.numero }}</h3>
        <form @submit.prevent="confirmarReserva" class="reserva-form">
          <div class="form-group">
            <label for="fecha-inicio">Fecha y Hora de Inicio:</label>
            <input
              id="fecha-inicio"
              v-model="reservaForm.fechaInicio"
              type="datetime-local"
              required
            />
          </div>
          
          <div class="form-group">
            <label for="duracion">Duración (horas):</label>
            <select id="duracion" v-model="reservaForm.duracion">
              <option value="1">1 hora</option>
              <option value="2">2 horas</option>
              <option value="4">4 horas</option>
              <option value="8">8 horas</option>
              <option value="24">1 día</option>
            </select>
          </div>
          
          <div class="reserva-preview">
            <h4>Resumen de la Reserva:</h4>
            <p><strong>Plaza:</strong> {{ selectedPlaza?.numero }} ({{ selectedPlaza?.tipo }})</p>
            <p><strong>Precio por hora:</strong> {{ selectedPlaza?.precioHora }}€</p>
            <p><strong>Total estimado:</strong> {{ totalEstimado }}€</p>
          </div>
          
          <div class="form-actions">
            <button type="button" @click="showReservaModal = false" class="cancel-button">
              Cancelar
            </button>
            <button type="submit" class="confirmar-button">
              Confirmar Reserva
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';
import { usePlazasStore } from '@/stores/plazas';
import { useReservasStore } from '@/stores/reservas';
import type { Plaza, PlazaQueryParameters, ReservaCreateDto } from '@/types';

const router = useRouter();
const authStore = useAuthStore();
const plazasStore = usePlazasStore();
const reservasStore = useReservasStore();

// Estado de los filtros
const filters = reactive<PlazaQueryParameters>({
  planta: undefined,
  zona: undefined,
  tipo: undefined,
  disponible: undefined,
  precioMin: undefined,
  precioMax: undefined
});

// Estado del modal de reserva
const showReservaModal = ref(false);
const selectedPlaza = ref<Plaza | null>(null);
const reservaForm = reactive({
  fechaInicio: '',
  duracion: 1
});

// Computed properties
const filteredPlazas = computed(() => {
  let result = plazasStore.plazas;

  if (filters.planta !== undefined) {
    result = result.filter(p => p.planta === Number(filters.planta));
  }
  
  if (filters.zona) {
    result = result.filter(p => p.zona === filters.zona);
  }
  
  if (filters.tipo) {
    result = result.filter(p => p.tipo === filters.tipo);
  }
  
  if (filters.disponible !== undefined) {
    result = result.filter(p => p.disponible === (filters.disponible === 'true'));
  }
  
  if (filters.precioMin) {
    result = result.filter(p => p.precioHora >= Number(filters.precioMin));
  }
  
  if (filters.precioMax) {
    result = result.filter(p => p.precioHora <= Number(filters.precioMax));
  }
  
  return result;
});

const totalGanado = computed(() => {
  return reservasStore.reservas
    .filter(r => r.estado === 'Finalizada')
    .reduce((total, r) => total + r.totalAPagar, 0)
    .toFixed(2);
});

const totalEstimado = computed(() => {
  if (!selectedPlaza.value) return 0;
  return (selectedPlaza.value.precioHora * reservaForm.duracion).toFixed(2);
});

// Métodos
const applyFilters = () => {
  // Los filtros se aplican automáticamente por computed
  console.log('Filtros aplicados:', filters);
};

const clearFilters = () => {
  Object.keys(filters).forEach(key => {
    (filters as any)[key] = undefined;
  });
};

const refreshPlazas = async () => {
  try {
    await plazasStore.fetchPlazas();
  } catch (error) {
    console.error('Error al refrescar plazas:', error);
  }
};

const reservarPlaza = (plaza: Plaza) => {
  selectedPlaza.value = plaza;
  reservaForm.fechaInicio = new Date().toISOString().slice(0, 16);
  showReservaModal.value = true;
};

const verDetallesPlaza = (plaza: Plaza) => {
  // TODO: Implementar vista de detalles
  alert(`Detalles de la plaza ${plaza.numero}`);
};

const confirmarReserva = async () => {
  if (!selectedPlaza.value) return;
  
  try {
    const reservaData: ReservaCreateDto = {
      plazaId: selectedPlaza.value.id,
      fechaInicio: reservaForm.fechaInicio
    };
    
    await reservasStore.createReserva(reservaData);
    
    // Cerrar modal y refrescar datos
    showReservaModal.value = false;
    await refreshPlazas();
    
    alert('¡Reserva creada exitosamente!');
  } catch (error) {
    console.error('Error al crear reserva:', error);
    alert('Error al crear la reserva');
  }
};

const handleLogout = () => {
  authStore.logout();
  router.push('/login');
};

// Lifecycle
onMounted(async () => {
  try {
    await Promise.all([
      plazasStore.fetchPlazas(),
      reservasStore.fetchReservas()
    ]);
  } catch (error) {
    console.error('Error al cargar datos iniciales:', error);
  }
});
</script>

<style scoped>
.dashboard-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 20px;
}

.dashboard-header {
  background: white;
  border-radius: 16px;
  padding: 25px 30px;
  margin-bottom: 25px;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 20px;
}

.dashboard-header h1 {
  color: #333;
  margin: 0;
  font-size: 2.2rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 12px;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 20px;
  flex-wrap: wrap;
}

.user-info span {
  color: #666;
  font-size: 1.1rem;
  font-weight: 500;
}

.logout-button {
  background: linear-gradient(135deg, #dc3545 0%, #c82333 100%);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  font-size: 1rem;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(220, 53, 69, 0.3);
}

.logout-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(220, 53, 69, 0.4);
}

/* Estadísticas */
.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 25px;
  margin-bottom: 30px;
}

.stat-card {
  background: white;
  border-radius: 16px;
  padding: 25px;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
  gap: 20px;
  transition: all 0.3s ease;
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 35px rgba(0, 0, 0, 0.15);
}

.stat-icon {
  font-size: 2.5rem;
  width: 60px;
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 50%;
  color: white;
}

.stat-content h3 {
  margin: 0;
  font-size: 2.2rem;
  color: #333;
  font-weight: 700;
  line-height: 1;
}

.stat-content p {
  margin: 8px 0 0 0;
  color: #666;
  font-size: 1rem;
  font-weight: 500;
}

/* Filtros */
.filters-section {
  background: white;
  border-radius: 16px;
  padding: 30px;
  margin-bottom: 30px;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
}

.filters-section h2 {
  margin: 0 0 25px 0;
  color: #333;
  font-size: 1.8rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 12px;
}

.filters-section h2::before {
  content: "🔍";
  font-size: 1.5rem;
}

.filters-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 20px;
  margin-bottom: 25px;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.filter-group label {
  font-weight: 600;
  color: #333;
  font-size: 0.95rem;
}

.filter-group select,
.filter-group input {
  padding: 12px 16px;
  border: 2px solid #e1e5e9;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.3s ease;
  background: white;
}

.filter-group select:focus,
.filter-group input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.filter-actions {
  display: flex;
  gap: 15px;
  flex-wrap: wrap;
}

.apply-filters-button,
.clear-filters-button {
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.apply-filters-button {
  background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(40, 167, 69, 0.3);
}

.apply-filters-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(40, 167, 69, 0.4);
}

.clear-filters-button {
  background: linear-gradient(135deg, #6c757d 0%, #495057 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(108, 117, 125, 0.3);
}

.clear-filters-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(108, 117, 125, 0.4);
}

/* Lista de plazas */
.plazas-section {
  background: white;
  border-radius: 16px;
  padding: 30px;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 25px;
  flex-wrap: wrap;
  gap: 20px;
}

.section-header h2 {
  margin: 0;
  color: #333;
  font-size: 1.8rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 12px;
}

.section-header h2::before {
  content: "🚗";
  font-size: 1.5rem;
}

.refresh-button {
  background: linear-gradient(135deg, #17a2b8 0%, #20c997 100%);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  font-size: 1rem;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(23, 162, 184, 0.3);
}

.refresh-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(23, 162, 184, 0.4);
}

.plazas-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 25px;
}

.plaza-card {
  border: 2px solid #e1e5e9;
  border-radius: 16px;
  padding: 25px;
  transition: all 0.3s ease;
  background: white;
  position: relative;
  overflow: hidden;
}

.plaza-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.plaza-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 35px rgba(0, 0, 0, 0.15);
}

.plaza-card.ocupada {
  border-color: #dc3545;
  background: linear-gradient(135deg, #fff5f5 0%, #f8f9fa 100%);
}

.plaza-card.ocupada::before {
  background: linear-gradient(135deg, #dc3545 0%, #c82333 100%);
}

.plaza-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.plaza-header h3 {
  margin: 0;
  color: #333;
  font-size: 1.5rem;
  font-weight: 700;
}

.plaza-status {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.plaza-status.disponible {
  background: linear-gradient(135deg, #d4edda 0%, #c3e6cb 100%);
  color: #155724;
  border: 1px solid #c3e6cb;
}

.plaza-status.ocupada {
  background: linear-gradient(135deg, #f8d7da 0%, #f5c6cb 100%);
  color: #721c24;
  border: 1px solid #f5c6cb;
}

.plaza-details {
  margin-bottom: 25px;
}

.plaza-details p {
  margin: 8px 0;
  color: #666;
  font-size: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.plaza-details p strong {
  color: #333;
  font-weight: 600;
}

.plaza-actions {
  display: flex;
  gap: 12px;
}

.reservar-button,
.detalles-button {
  flex: 1;
  padding: 12px 16px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.reservar-button {
  background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(40, 167, 69, 0.3);
}

.reservar-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(40, 167, 69, 0.4);
}

.detalles-button {
  background: linear-gradient(135deg, #6c757d 0%, #495057 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(108, 117, 125, 0.3);
}

.detalles-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(108, 117, 125, 0.4);
}

/* Estados */
.loading,
.error-message,
.no-results {
  text-align: center;
  padding: 60px 40px;
  color: #666;
  font-size: 1.1rem;
}

.error-message {
  color: #dc3545;
  background: #f8f9fa;
  border-radius: 12px;
  border: 1px solid #f5c6cb;
}

.no-results {
  background: #f8f9fa;
  border-radius: 12px;
  border: 1px solid #e1e5e9;
}

/* Modal */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  backdrop-filter: blur(5px);
}

.modal {
  background: white;
  border-radius: 20px;
  padding: 35px;
  width: 90%;
  max-width: 550px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.modal h3 {
  margin: 0 0 25px 0;
  text-align: center;
  color: #333;
  font-size: 1.8rem;
  font-weight: 600;
}

.reserva-form {
  display: flex;
  flex-direction: column;
  gap: 25px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.form-group label {
  font-weight: 600;
  color: #333;
  font-size: 1rem;
}

.form-group input,
.form-group select {
  padding: 14px 18px;
  border: 2px solid #e1e5e9;
  border-radius: 10px;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.reserva-preview {
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  padding: 20px;
  border-radius: 12px;
  border: 1px solid #e1e5e9;
}

.reserva-preview h4 {
  margin: 0 0 15px 0;
  color: #333;
  font-size: 1.2rem;
  font-weight: 600;
}

.reserva-preview p {
  margin: 8px 0;
  color: #666;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.reserva-preview p strong {
  color: #333;
}

.form-actions {
  display: flex;
  gap: 15px;
  justify-content: flex-end;
  margin-top: 10px;
}

.cancel-button,
.confirmar-button {
  padding: 14px 28px;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 600;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.cancel-button {
  background: linear-gradient(135deg, #6c757d 0%, #495057 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(108, 117, 125, 0.3);
}

.cancel-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(108, 117, 125, 0.4);
}

.confirmar-button {
  background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(40, 167, 69, 0.3);
}

.confirmar-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(40, 167, 69, 0.4);
}

/* Responsividad */
@media (max-width: 768px) {
  .dashboard-container {
    padding: 15px;
  }
  
  .dashboard-header {
    flex-direction: column;
    text-align: center;
    padding: 20px;
  }
  
  .dashboard-header h1 {
    font-size: 1.8rem;
  }
  
  .stats-grid {
    grid-template-columns: 1fr;
    gap: 20px;
  }
  
  .filters-grid {
    grid-template-columns: 1fr;
    gap: 15px;
  }
  
  .plazas-grid {
    grid-template-columns: 1fr;
    gap: 20px;
  }
  
  .modal {
    padding: 25px;
    margin: 20px;
  }
  
  .form-actions {
    flex-direction: column;
  }
}

@media (max-width: 480px) {
  .dashboard-header h1 {
    font-size: 1.5rem;
  }
  
  .stat-card {
    padding: 20px;
  }
  
  .stat-icon {
    font-size: 2rem;
    width: 50px;
    height: 50px;
  }
  
  .stat-content h3 {
    font-size: 1.8rem;
  }
}
</style>


