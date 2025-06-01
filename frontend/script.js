const apiUrl = 'http://localhost:8080/players';

window.onload = async () => {
  const leftList = document.getElementById("leftList");

  try {
    const response = await fetch(apiUrl);
    const players = await response.json();

    players.forEach(player => {
      const option = document.createElement("option");
      option.value = player.name;
      option.text = player.name;
      leftList.add(option);
    });
  } catch (error) {
    console.error("Error loading players:", error);
    const errorOption = document.createElement("option");
    errorOption.text = "Failed to load players";
    leftList.add(errorOption);
  }
};

function moveSelected(from, to) {
  const fromList = document.getElementById(from + "List");
  const toList = document.getElementById(to + "List");

  const selectedOptions = Array.from(fromList.selectedOptions);
  selectedOptions.forEach(option => {
    fromList.remove(option.index);
    toList.add(option);
  });
}
