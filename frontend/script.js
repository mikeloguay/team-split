const API_BASE_URL =
    window.location.hostname === "localhost" || window.location.protocol === "file:"
        ? "http://localhost:8080"
        : "https://teamsplit-api.onrender.com";

document.addEventListener("DOMContentLoaded", async () => {
    const playersList = document.getElementById("players-list");
    const playersCount = document.getElementById("players-count");
    const selectedCount = document.getElementById("selected-count");
    const teamsDiv = document.getElementById("teams");
    const splitButton = document.getElementById("split-button");
    const errorMessage = document.getElementById("error-message");

    let players = [];

    async function fetchWithRetry(url, options = {}, retries = 3, delay = 5000) {
        showSpinner();
        for (let attempt = 1; attempt <= retries; attempt++) {
            try {
                const response = await fetch(url, options);
                if (!response.ok) {
                    if (response.status >= 400 && response.status < 500) {
                        return response;
                    }
                    throw new Error(`HTTP ${response.status}`);
                }
                return response;
            } catch (err) {
                if (attempt === retries) 
                {
                    throw err;
                }
                    
                await new Promise(res => setTimeout(res, delay));
            } finally {
                hideSpinner();
            }
        }
    }

    const fetchPlayers = async () => {
        try {
            const response = await fetchWithRetry(`${API_BASE_URL}/players`);
            if (!response.ok) {
                const problem = await response.json();
                throw new Error(problem.title || `Error ${response.status}: ${response.statusText}`);
            }
            const data = await response.json();
            players = data.players;
            renderPlayers();
        } catch (error) {
            showError(error.message);
        }
    };

    const renderPlayers = () => {
        playersList.innerHTML = "";
        players.forEach(player => {
            const checkbox = document.createElement("input");
            checkbox.type = "checkbox";
            checkbox.value = player;
            checkbox.id = `player-${player}`;
            checkbox.addEventListener("change", updateSelectedCount);

            const label = document.createElement("label");
            label.textContent = player;
            label.setAttribute("for", `player-${player}`);

            const div = document.createElement("div");
            div.appendChild(checkbox);
            div.appendChild(label);

            playersList.appendChild(div);
        });

        updatePlayersCount();
        updateSelectedCount();
    };

    const updatePlayersCount = () => {
        playersCount.textContent = players.length;
    };

    const updateSelectedCount = () => {
        const selectedPlayers = document.querySelectorAll("#players-list input:checked");
        selectedCount.textContent = selectedPlayers.length;
    };

    splitButton.addEventListener("click", async () => {
        hideError();
        teamsDiv.innerHTML = "";
        try {
            const selectedPlayers = Array.from(document.querySelectorAll("#players-list input:checked"))
                                        .map(input => input.value);

            const response = await fetchWithRetry(`${API_BASE_URL}/players/split`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ players: selectedPlayers })
            });

            if (!response.ok) {
                const problem = await response.json();
                throw new Error(problem.title || `Error ${response.status}: ${response.statusText}`);
            }

            const data = await response.json();
            renderTeams(data);
        } catch (error) {
            showError(error.message);
        }
    });

    function renderTeams(data) {
        // data.team1.name, data.team1.players, data.team2.name, data.team2.players
        const team1 = data.team1.players;
        const team2 = data.team2.players;

        // Calcula el máximo de jugadores para alinear filas
        const maxPlayers = Math.max(team1.length, team2.length);

        // Genera HTML para cada equipo
        let team1Row = '';
        let team2Row = '';
        for (let i = 0; i < maxPlayers; i++) {
            team1Row += team1[i]
                ? `<div class="player-shirt"><div class="shirt team1"> </div><div class="player-name">${team1[i]}</div></div>`
                : `<div class="player-shirt"></div>`;
            team2Row += team2[i]
                ? `<div class="player-shirt"><div class="shirt team2"> </div><div class="player-name">${team2[i]}</div></div>`
                : `<div class="player-shirt"></div>`;
        }

        teamsDiv.innerHTML = `
            <div class="football-field">
                <div class="team-row">${team1Row}</div>
                <div class="team-row">${team2Row}</div>
            </div>
        `;
    }

    const showError = (message) => {
        errorMessage.textContent = message;
        errorMessage.style.display = "block";
    };

    const hideError = (message) => {
        errorMessage.textContent = "";
        errorMessage.style.display = "none";
    };

    function showSpinner() {
        document.getElementById("spinner").style.display = "block";
    }
    function hideSpinner() {
        document.getElementById("spinner").style.display = "none";
    }

    fetchPlayers();
});
