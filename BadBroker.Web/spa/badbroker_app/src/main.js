import { createApp } from 'vue';
import App from './App.vue'
import './registerServiceWorker'
import PrimeVue from 'primevue/config';
import Button from 'primevue/button';
import Toast from 'primevue/toast';
import Calendar from 'primevue/calendar';
import InputNumber from 'primevue/inputnumber';
import ToastService from 'primevue/toastservice';


import "primevue/resources/themes/lara-light-indigo/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";

const app = createApp(App);
app.use(PrimeVue);
app.use(ToastService);
app.component('ButtonSubmit', Button);
app.component('ToastMessage', Toast);
app.component('CalendarPicker', Calendar);
app.component('InputNumber', InputNumber);

app.mount('#app')

