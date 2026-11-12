// =====================================================================
// Avatar.jsx – kerek profilkép. Ha nincs kép, a felhasználónév első
// betűje látszik egy színes körben. A szín a felhasználó azonosítójából
// számolódik, így mindenkinek mindig ugyanaz marad.
// =====================================================================
export default function Avatar({ username, id = 0, src = null, size = 42 }) {
  // Közös méretek (a betűméret a kör méretének ~42%-a).
  const style = { width: size, height: size, fontSize: size * 0.42 };
 
  // Van profilkép: azt mutatjuk.
  if (src) return <img className="avatar" src={src} alt="" style={style} />;
 
  // Nincs: színes kör + kezdőbetű. A hue (színárnyalat) 0–359 közötti szám az id-ból.
  const hue = (id * 67) % 360;
  return (
    <span className="avatar" style={{ ...style, background: `hsl(${hue} 45% 38%)` }}>
      {username[0].toUpperCase()}
    </span>
  );
}