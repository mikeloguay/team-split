document.addEventListener("DOMContentLoaded", async () => {
    const playersList = document.getElementById("players-list");
    const selectedCount = document.getElementById("selected-count");
    const teamsDiv = document.getElementById("teams");
    const splitButton = document.getElementById("split-button");
    const errorMessage = document.getElementById("error-message");

    let players = [];

    // Cargar jugadores al inicio
    const fetchPlayers = async () => {
        try {
            const response = await fetch("https://team-split.onrender.com/api/players");
            if (!response.ok) throw new Error(`Error ${response.status}: ${response.statusText}`);
            
            const data = await response.json();
            players = data.players;
            renderPlayers();
        } catch (error) {
            showError(error.message);
        }
    };

    // Renderizar lista de jugadores
    const renderPlayers = () => {
        playersList.innerHTML = "";
        players.forEach(player => {
            const checkbox = document.createElement("input");
            checkbox.type = "checkbox";
            checkbox.value = player;
            checkbox.checked = true;
            checkbox.addEventListener("change", updateSelectedCount);

            const label = document.createElement("label");
            label.textContent = player;

            const div = document.createElement("div");
            div.appendChild(checkbox);
            div.appendChild(label);

            playersList.appendChild(div);
        });

        updateSelectedCount();
    };

    // Actualizar contador de seleccionados
    const updateSelectedCount = () => {
        const selectedPlayers = document.querySelectorAll("#players-list input:checked");
        selectedCount.textContent = selectedPlayers.length;
    };

    // Dividir equipos al hacer clic en el botón
    splitButton.addEventListener("click", async () => {
        try {
            const selectedPlayers = Array.from(document.querySelectorAll("#players-list input:checked"))
                                        .map(input => input.value);

            const response = await fetch("https://team-split.onrender.com/api/players/split", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ players: selectedPlayers })
            });

            if (!response.ok) throw new Error(`Error ${response.status}: ${response.statusText}`);

            const data = await response.json();
            renderTeams(data);
        } catch (error) {
            showError(error.message);
        }
    });

    // Renderizar equipos
    const renderTeams = (data) => {
        teamsDiv.innerHTML = `<h3>${data.team1.name}</h3><p>${data.team1.players.join(", ")}</p>
                              <h3>${data.team2.name}</h3><p>${data.team2.players.join(", ")}</p>`;
    };

    // Mostrar mensaje de error
    const showError = (message) => {
        errorMessage.textContent = message;
        errorMessage.style.display = "block";
    };

    fetchPlayers();
});
