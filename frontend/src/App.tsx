import "./App.css";
import CookieConsent from "react-cookie-consent";
import Fingerprint from "./Fingerprint";
import ProjectsPage from "./pages/ProjectsPage";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import DonatePage from "./pages/DonatePage";
import CartPage from "./pages/CartPage";
import { CartProvider } from "./context/CartContext";
import AdminProjectsPage from "./pages/AdminProjectsPage";

function App() {
  return (
    <>
      <CartProvider>
        <Router>
          <Routes>
            <Route path="/" element={<ProjectsPage />} />
            <Route path="/projects" element={<ProjectsPage />} />
            <Route
              path="/donate/:projectName/:projectId"
              element={<DonatePage />}
            />
            <Route path="/cart" element={<CartPage />} />
            <Route path="/adminprojects" element={<AdminProjectsPage />} />
          </Routes>
        </Router>
      </CartProvider>

      <CookieConsent>
        This website uses cookies to enhance the user experience.
      </CookieConsent>
      <Fingerprint />
    </>
  );
}

export default App;
