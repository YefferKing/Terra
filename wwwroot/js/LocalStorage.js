// Al iniciar sesión, establece un valor en sessionStorage
sessionStorage.setItem("user_session_valid", "true");
sessionStorage.setItem("DASHBOARDMENSAJE", "Bienvenid@ al Dashboard!");

//cerrar sesion
sessionStorage.removeItem("user_session_valid");
sessionStorage.removeItem("DASHBOARDMENSAJE");