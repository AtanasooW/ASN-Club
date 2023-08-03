var connection = new signalR.HubConnectionBuilder().withUrl("/commentsHub").build();

connection.start()
    .then(function () {
        // Enable the "Add Comment" button after the connection is established.
        document.getElementById("addComment").removeAttribute("disabled");

        // In your JavaScript code
        connection.on("ReceiveComment", (username, comment, currentTime) => {
            // Create a new comment element and append it to the comments section
            const newCommentElement = document.createElement("div");
            newCommentElement.innerHTML =
                `
            <div class="card mb-4">
                <div class="card-body">
                    <p>${comment}</p>

                    <div class="d-flex justify-content-between">
                        <div class="d-flex flex-row align-items-center">
                            <p class="small mb-0 ms-2">${username}</p>
                        </div>
                        <div class="d-flex flex-row align-items-center">
                            <p class="small text-muted mb-0">${currentTime}</p>
                        </div>
                    </div>
                </div>
            </div>
            `;
            // Append the new comment at the beginning to show the latest comment first
            document.getElementById("commentsList").prepend(newCommentElement);
        });
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

document.getElementById("addComment").addEventListener("click", () => {
    const commentInput = document.getElementById("commentInput");
    const userInput = document.getElementById("usernameInput");
    const currentTime = new Date().toISOString();
    const username = userInput.value; // Use .value instead of .innerText
    const comment = commentInput.value;

    // Check if the comment is not empty before sending it to the server
    if (comment.trim() !== "") {
        // Call the server-side hub method to add the comment using SignalR
        document.getElementById("helpper").value = comment;
        connection.invoke("UploadComment", username, comment, currentTime)
            .catch(function (err) {
                console.error(err.toString());
            });
    }

    // Clear the comment input field after adding the comment
    commentInput.value = "";
});
