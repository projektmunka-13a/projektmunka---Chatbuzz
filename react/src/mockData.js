// =====================================================================
// mockData.js – KITALÁLT adatok, hogy a hírfolyam backend nélkül is
// kinézzen valahogy. A posztok mezői pontosan a backend PostDTO-ját
// követik (id, userId, username, profilePic, content, imageUrl,
// createdAt, likeCount, commentCount), így az összekötésnél csak a
// forrást kell cserélni (api.getPosts()), a kinézethez nem kell nyúlni.
// =====================================================================
 
// Segéd: "X perccel ezelőtti" időpontot ad vissza ISO szövegként.
const ago = (minutes) => new Date(Date.now() - minutes * 60000).toISOString();
 
// A "bejelentkezett" felhasználó (demó).
export const me = { id: 1, username: "te" };
 
// Egy színátmenetes kép, hogy a képes poszt is látszódjon (nincs külső kép).
const demoImage =
  "data:image/svg+xml;utf8," +
  encodeURIComponent(
    `<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 800 500'>
       <defs><linearGradient id='g' x1='0' y1='0' x2='1' y2='1'>
         <stop offset='0' stop-color='#6657f0'/><stop offset='1' stop-color='#4fd1e8'/>
       </linearGradient></defs>
       <rect width='800' height='500' fill='url(#g)'/>
     </svg>`
  );
 
// A hírfolyam kezdő posztjai (a legújabb elöl).
export const initialPosts = [
  {
    id: 5, userId: 3, username: "anna.k", profilePic: null,
    content: "Végre kész a vizsgaremek első verziója! Kicsit fáradt vagyok, de megérte.",
    imageUrl: demoImage, createdAt: ago(12), likeCount: 24, commentCount: 6,
  },
  {
    id: 4, userId: 4, username: "bence_dev", profilePic: null,
    content: "Tippet kérek: ti melyik szerkesztővel dolgoztok szívesebben, és miért?",
    imageUrl: null, createdAt: ago(95), likeCount: 9, commentCount: 14,
  },
  {
    id: 3, userId: 5, username: "zsofi", profilePic: null,
    content: "Hétvégén kirándulás a Börzsönyben. Aki szeretne jönni, írjon nekem!",
    imageUrl: null, createdAt: ago(60 * 7), likeCount: 31, commentCount: 3,
  },
  {
    id: 2, userId: 6, username: "marci", profilePic: null,
    content: "Ma végre sikerült összehoznom az első saját React komponensemet.",
    imageUrl: null, createdAt: ago(60 * 26), likeCount: 12, commentCount: 2,
  },
];
 
// Az "Ajánlott felhasználók" lista a jobb oldalon.
export const suggestions = [
  { id: 3, username: "anna.k" },
  { id: 4, username: "bence_dev" },
  { id: 5, username: "zsofi" },
  { id: 6, username: "marci" },
];