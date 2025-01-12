<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useToast } from 'primevue/usetoast'
import { SavingGoalService } from '@/services/SavingGoalService'
import { FilterMatchMode } from '@primevue/core/api'

const toast = useToast();

const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
});

interface SavingGoal {
  id?: string;
  name: string;
  targetAmount: number;
  currentAmount: number;
  deadline: Date;
  isCompleted: boolean;
  notes: string;
}

interface ServiceResponse {
  success: boolean;
  errors?: Array<{ message: string }>;
}

const initialSavingGoalState: SavingGoal = {
  id: undefined,
  name: '',
  targetAmount: 0,
  currentAmount: 0,
  deadline: new Date(),
  isCompleted: false,
  notes: ''
};

const savingGoals = ref<SavingGoal[]>([]);
const savingGoal = ref<SavingGoal>({ ...initialSavingGoalState });
const type = ref('new');
const savingGoalDialog = ref(false);
const deleteSavingGoalDialog = ref(false);
const showContributeDialog = ref(false);
const submitted = ref(false);
const contributeAmount = ref(0);

const showToast = (severity: string, summary: string, detail: string) => {
  toast.add({ severity, summary, detail, life: 3000 });
};

const validateSavingGoal = (goal: SavingGoal): string[] => {
  const errors: string[] = [];

  if (!goal.name?.trim()) errors.push('Name is required');
  if (!goal.targetAmount) errors.push('Target amount is required');
  if (goal.notes?.length > 350) {
    errors.push(`Notes cannot exceed 250 characters`);
  }

  return errors;
};

const fetchSavingGoals = async () => {
  try {
    savingGoals.value = await SavingGoalService.getSavingGoals();
  } catch (error) {
    console.error('Error fetching saving goals:', error);
    showToast('error', 'Error', 'Failed to fetch saving goals');
  }
};

const openNew = () => {
  type.value = 'new';
  savingGoal.value = { ...initialSavingGoalState };
  submitted.value = false;
  savingGoalDialog.value = true;
};

const handleServiceResponse = async (
  response: ServiceResponse,
  successMessage: string
) => {
  if (response.success) {
    showToast('success', 'Successful', successMessage);
    savingGoalDialog.value = false;
  } else {
    const errorMessage = response.errors?.[0]?.message ?? 'An error occurred';
    showToast('error', 'Error', errorMessage);
  }
};

const saveSavingGoal = async () => {
  submitted.value = true;
  const errorMessages = validateSavingGoal(savingGoal.value);

  if (errorMessages.length > 0) {
    errorMessages.forEach(message => showToast('error', 'Error', message));
    return;
  }

  const payload = {
    name: savingGoal.value.name.trim(),
    targetAmount: savingGoal.value.targetAmount,
    currentAmount: savingGoal.value.currentAmount,
    deadline: savingGoal.value.deadline,
    isCompleted: savingGoal.value.isCompleted,
    notes: savingGoal.value.notes.trim(),
  };

  try {
    debugger;
    const response = savingGoal.value.id
      ? await SavingGoalService.updateSavingGoal({ ...payload, id: savingGoal.value.id })
      : await SavingGoalService.createSavingGoal(payload);

    await handleServiceResponse(
      response,
      savingGoal.value.id ? 'Saving Goal Updated' : 'Saving Goal Created'
    );

    if (response.success) {
      const updatedGoal = response.data;
      if (savingGoal.value.id) {
        const index = savingGoals.value.findIndex(g => g.id === updatedGoal.id);
        if (index !== -1) {
          savingGoals.value[index] = updatedGoal;
        }
      } else {
        savingGoals.value.push(updatedGoal);
      }
    }

  } catch (error) {
    console.error('Error saving goal:', error);
    showToast('error', 'Error', 'Failed to save saving goal');
  }
};

const editSavingGoal = (goal: SavingGoal) => {
  type.value = 'edit';
  savingGoal.value = { ...goal };
  savingGoalDialog.value = true;
};

const confirmDeleteSavingGoal = (goal: SavingGoal) => {
  savingGoal.value = { ...goal };
  deleteSavingGoalDialog.value = true;
}

const contributeSavingGoal = (goal: SavingGoal) => {
  savingGoal.value = { ...goal };
  showContributeDialog.value = true;
}

const deleteSavingGoal = async () => {
  try {
    const response = await SavingGoalService.deleteSavingGoal(savingGoal.value.id );
    if (response?.success) {
      const index = savingGoals.value.findIndex(goal => goal.id === savingGoal.value.id);
      if (index !== -1) {
        savingGoals.value.splice(index, 1);
      }
      deleteSavingGoalDialog.value = false;
      showToast('success', 'Successful', 'Saving Goal Deleted');
    }
  } catch (error) {
    console.error('Error deleting saving goal:', error);
    showToast('error', 'Error', 'An unexpected error occurred while deleting the saving goal');
  }
};

const contribute = async() => {
  try {
    const response = await SavingGoalService.contributeSavingGoal(savingGoal.value.id, contributeAmount.value);
    if (response.success) {
      const updatedGoal = response.data;
      if (savingGoal.value.id) {
        const index = savingGoals.value.findIndex(g => g.id === updatedGoal.id);
        if (index !== -1) {
          savingGoals.value[index] = updatedGoal;
        }
      }
      contributeAmount.value = 0;
      showContributeDialog.value = false;
      showToast('success', 'Successful', 'Contributed successfully');
    }
  } catch (error) {
    showToast('error', 'Error', 'Failed to contribute');
  }
};

onMounted(() => {
  fetchSavingGoals();
});
</script>


<template>
  <div>
    <div class="card">
      <Toolbar class="mb-2">
        <template #start>
          <Button label="New" icon="pi pi-plus" severity="secondary" class="mr-2" @click="openNew" />
        </template>
        <template #end>
          <Button label="Export" icon="pi pi-upload" severity="secondary" @click="exportCSV" />
        </template>
      </Toolbar>

      <DataView :value="savingGoals" :layout="'grid'" :filters="filters" >
        <template #header>
          <div class="flex justify-between items-center">
            <h4 class="m-0 text-xl">Manage Saving Goals</h4>
            <InputText v-model="filters.global.value" placeholder="Search..." />
          </div>
        </template>
        <template #grid="slotProps">
          <div class="grid grid-cols-12 gap-4 mt-4">
            <div v-for="(item, index) in slotProps.items" :key="index" class="col-span-12 sm:col-span-6 md:col-span-4 xl:col-span-4 p-2">
              <div class="p-6 border border-surface-200 dark:border-surface-700 bg-surface-0 dark:bg-surface-900 rounded flex flex-col">
                <div>
                  <div class="flex flex-row justify-between items-start gap-2">
                    <div>
                      <div class="text-lg font-medium mt-1">{{ item.name }}</div>
                    </div>
                    <div class="bg-surface-100 dark:bg-surface-900 p-1" style="border-radius: 30px">
                      <div class="bg-surface-0 dark:bg-surface-800 flex items-center gap-2 justify-center py-1 px-2" style="border-radius: 10px; box-shadow: 0px 1px 2px 0px rgba(0, 0, 0, 0.04), 0px 1px 2px 0px rgba(0, 0, 0, 0.06)">
                        <span class="text-surface-900 dark:text-surface-100 font-medium text-sm">
                          {{ item.isCompleted ? 'Completed' : 'In progress' }}
                        </span>
                        <i v-if="item.isCompleted"
                           class="pi pi-check-circle text-green-400 dark:text-green-500"></i>
                        <i v-else
                           class="pi pi-clock text-blue-400 dark:text-blue-500"></i>
                      </div>
                    </div>

                  </div>

                  <div class="flex flex-col gap-6 mt-6">

                    <div class="grid grid-cols-2 gap-4">
                      <div class="bg-surface-50 dark:bg-surface-800 p-3 rounded-lg">
                        <div class="text-sm text-surface-600 dark:text-surface-400">Current</div>
                        <div class="text-lg font-bold dark:text-white">{{ $formatters.formatCurrency(item.currentAmount) }}</div>
                      </div>
                      <div class="bg-surface-50 dark:bg-surface-800 p-3 rounded-lg">
                        <div class="text-sm text-surface-600 dark:text-surface-400">Target</div>
                        <div class="text-lg font-bold dark:text-white">{{ $formatters.formatCurrency(item.targetAmount) }}</div>
                      </div>
                    </div>

                    <div class="bg-surface-50 dark:bg-surface-800 p-3 rounded-lg">
                      <div class="text-sm text-surface-600 dark:text-surface-400">Deadline</div>
                      <div class="text-base font-medium dark:text-white">{{ $formatters.formatDate(item.deadline) }}</div>
                    </div>

                    <div v-if="item.notes" class="bg-surface-50 dark:bg-surface-800 p-3 rounded-lg">
                      <div class="text-sm text-surface-600 dark:text-surface-400">Notes</div>
                      <div class="text-sm mt-1 dark:text-surface-200">{{ item.notes }}</div>
                    </div>

                    <div class="flex gap-2">
                      <div class="flex gap-2">
                        <Button icon="pi pi-trash" severity="danger" outlined class="w-full" v-tooltip="'Delete Saving Goal'" @click="confirmDeleteSavingGoal(item)" />
                        <Button icon="pi pi-pen-to-square" severity="primary" outlined class="w-full" v-tooltip="'Edit Saving Goal'" @click="editSavingGoal(item)" />
                      </div>
                      <Button label="Contribute" icon="pi pi-money-bill" outlined class="ml-auto" @click="contributeSavingGoal(item)"/>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </template>
      </DataView>

      <Dialog
        v-model:visible="savingGoalDialog"
        :style="{ width: '450px' }"
        :header="type ==='edit' ? 'Edit Saving Goal' : 'Add new Saving Goal'"
        :modal="true"
      >
        <div>
          <div>
            <label for="name" class="block font-bold mb-2">Name</label>
            <InputText id="name" v-model="savingGoal.name" required autofocus class="w-full" />
            <div v-if="submitted && !savingGoal.name" class="p-error">Name is required.</div>
          </div>

          <div>
            <label for="targetAmount" class="block font-bold mt-2 mb-2">Target Amount</label>
            <InputNumber id="targetAmount" mode="currency" currency="EUR" locale="de-DE" v-model="savingGoal.targetAmount" required autofocus class="w-full"  />
            <div v-if="submitted && !savingGoal.targetAmount" class="p-error">Target amount is required.</div>
          </div>

          <div>
            <label v-if="!savingGoal.id" for="currentAmount" class="block font-bold mt-2 mb-2">Current Amount</label>
            <InputNumber v-if="!savingGoal.id" id="currentAmount" mode="currency" currency="EUR" locale="de-DE"  v-model="savingGoal.currentAmount" required autofocus class="w-full" />
          </div>

          <div>
            <label for="deadline" class="block font-bold mt-2 mb-2">Deadline</label>
            <DatePicker id="deadline" v-model="savingGoal.deadline" class="w-full" />
            <div v-if="submitted && !savingGoal.deadline" class="p-error">Deadline is required.</div>
          </div>

          <div>
            <label for="notes" class="block font-bold mt-2 mb-2">Notes</label>
            <Textarea id="notes" v-model="savingGoal.notes" rows="3" cols="20" class="w-full" />
            <div v-if="submitted && savingGoal.notes && savingGoal.notes.length > 250" class="p-error">Notes cannot exceed 250 characters.</div>
          </div>

        </div>

        <template #footer>
          <Button label="Cancel" icon="pi pi-times" text @click="savingGoalDialog = false" />
          <Button label="Save" icon="pi pi-check" @click="saveSavingGoal" />
        </template>
      </Dialog>

      <Dialog
        v-model:visible="deleteSavingGoalDialog"
        :style="{ width: '450px' }"
        header="Confirm"
        :modal="true"
      >
        <p>Are you sure you want to delete this saving goal?</p>
        <template #footer>
          <Button label="No" icon="pi pi-times" text @click="deleteSavingGoalDialog = false" />
          <Button label="Yes" icon="pi pi-check" severity="danger" @click="deleteSavingGoal()" />
        </template>
      </Dialog>

      <Dialog v-model:visible="showContributeDialog" header="Contribute" modal>
        <InputNumber v-model="contributeAmount" mode="currency" currency="EUR" locale="de-DE" placeholder="Amount" />
        <Button label="Confirm" class="ml-2" @click="contribute()" />
      </Dialog>

    </div>
  </div>
</template>

<style scoped>

</style>
