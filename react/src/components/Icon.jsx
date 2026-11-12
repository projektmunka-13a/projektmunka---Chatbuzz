// =====================================================================
// Icon.jsx – egyszerű vonalas ikonok SVG-ként (nem kell külső csomag).
// Használat: <Icon name="home" />  vagy  <Icon name="heart" filled />
// A "currentColor" miatt az ikon átveszi a szülő szövegszínét.
// =====================================================================
const PATHS = {
  home: <path d="M3 10.5 12 3l9 7.5V20a1 1 0 0 1-1 1h-5v-6H9v6H4a1 1 0 0 1-1-1z" />,
  search: (
    <>
      <circle cx="11" cy="11" r="7" />
      <path d="m20 20-3.5-3.5" />
    </>
  ),
  message: <path d="M21 12a8 8 0 0 1-11.6 7.1L4 20l1-4.6A8 8 0 1 1 21 12z" />,
  bell: (
    <>
      <path d="M6 9a6 6 0 1 1 12 0c0 6 2 7.5 2 7.5H4S6 15 6 9z" />
      <path d="M10 20a2 2 0 0 0 4 0" />
    </>
  ),
  user: (
    <>
      <circle cx="12" cy="8" r="4" />
      <path d="M4 21a8 8 0 0 1 16 0" />
    </>
  ),
  heart: <path d="M12 20.5s-8-4.7-8-10.2A4.6 4.6 0 0 1 12 7.6a4.6 4.6 0 0 1 8 2.7c0 5.5-8 10.2-8 10.2z" />,
  logout: (
    <>
      <path d="M9 21H5a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1h4" />
      <path d="m16 17 5-5-5-5" />
      <path d="M21 12H9" />
    </>
  ),
};
 
// size: méret pixelben; filled: kitöltött (pl. kedvelt szív)
export default function Icon({ name, size = 24, filled = false }) {
  return (
    <svg
      viewBox="0 0 24 24"
      width={size}
      height={size}
      fill={filled ? "currentColor" : "none"}
      stroke="currentColor"
      strokeWidth="1.8"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true" // díszítő, a képernyőolvasó kihagyja (a gombnak van szövege)
    >
      {PATHS[name]}
    </svg>
  );
}