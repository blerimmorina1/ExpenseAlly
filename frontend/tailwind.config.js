module.exports = {
  darkMode: 'class',
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {
      screens: {
        xs: '0px', // Extra small screens (default starting point)
        sm: '480px', // Small screens
        md: '768px', // Medium screens
        lg: '1024px', // Large screens
        xl: '1280px', // Extra large screens
        '2xl': '1536px', // 2x Extra large screens
      },
    },
  },
  plugins: [],
};
