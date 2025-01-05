import SignalRService from '@/services/SignalRService';

const baseURL = import.meta.env.VITE_API_BASE_URL;

const signalRHubUrl = `${baseURL}/notificationHub`; 
const signalRService = new SignalRService(signalRHubUrl);

export default signalRService;