import { computed, reactive, onMounted } from 'vue';

interface LayoutConfig {
  preset: string;
  primary: string;
  surface: string | null;
  darkTheme: boolean;
  menuMode: 'static' | 'overlay';
}

interface LayoutState {
  staticMenuDesktopInactive: boolean;
  overlayMenuActive: boolean;
  profileSidebarVisible: boolean;
  configSidebarVisible: boolean;
  staticMenuMobileActive: boolean;
  menuHoverActive: boolean;
  activeMenuItem: any | null;
}

const layoutConfig = reactive<LayoutConfig>({
  preset: 'Aura',
  primary: 'emerald',
  surface: null,
  darkTheme: false, // Default value
  menuMode: 'static'
});

const layoutState = reactive<LayoutState>({
  staticMenuDesktopInactive: false,
  overlayMenuActive: false,
  profileSidebarVisible: false,
  configSidebarVisible: false,
  staticMenuMobileActive: false,
  menuHoverActive: false,
  activeMenuItem: null
});

export function useLayout() {
  // Initialize dark mode from localStorage
  onMounted(() => {
    const savedDarkMode = localStorage.getItem('darkMode');
    if (savedDarkMode !== null) {
      layoutConfig.darkTheme = JSON.parse(savedDarkMode);
      if (layoutConfig.darkTheme) {
        document.documentElement.classList.add('app-dark');
      }
    }
  });

  const setActiveMenuItem = (item: any): void => {
    layoutState.activeMenuItem = item.value || item;
  };

  const toggleDarkMode = (): void => {
    if (!document.startViewTransition) {
      executeDarkModeToggle();
      return;
    }

    document.startViewTransition(() => executeDarkModeToggle());
  };

  const executeDarkModeToggle = (): void => {
    layoutConfig.darkTheme = !layoutConfig.darkTheme;
    // Save to localStorage
    localStorage.setItem('darkMode', JSON.stringify(layoutConfig.darkTheme));
    document.documentElement.classList.toggle('app-dark');
  };

  const toggleMenu = (): void => {
    if (layoutConfig.menuMode === 'overlay') {
      layoutState.overlayMenuActive = !layoutState.overlayMenuActive;
    }

    if (window.innerWidth > 991) {
      layoutState.staticMenuDesktopInactive = !layoutState.staticMenuDesktopInactive;
    } else {
      layoutState.staticMenuMobileActive = !layoutState.staticMenuMobileActive;
    }
  };

  const isSidebarActive = computed((): boolean =>
    layoutState.overlayMenuActive || layoutState.staticMenuMobileActive
);

  const isDarkTheme = computed((): boolean => layoutConfig.darkTheme);

  const getPrimary = computed((): string => layoutConfig.primary);

  const getSurface = computed((): string | null => layoutConfig.surface);

  return {
    layoutConfig,
    layoutState,
    toggleMenu,
    isSidebarActive,
    isDarkTheme,
    getPrimary,
    getSurface,
    setActiveMenuItem,
    toggleDarkMode
  };
}
