﻿const home = document.querySelector("#home-nav"),
    search = document.querySelector("#search-nav"),
    notifs = document.querySelector("#notifs-nav"),
    post = document.querySelector("#post-nav"),
    settings = document.querySelector("#settings-nav"),
    profile = document.querySelector("#profile-nav"),
    navItems = document.querySelectorAll(".nav-item");;

window.addEventListener("load", () => {
    removeAllActives();
    let url = window.location.href.toLowerCase();
    if (url.includes("home")) {
        home.classList.add("active");
    } else if (url.includes("search")) {
        search.classList.add("active");
    } else if (url.includes("notifications")) {
        notifs.classList.add("active");
    } else if (url.includes("post")) {
        post.classList.add("active");
    } else if (url.includes("settings")) {
        settings.classList.add("active");
    } else if (url.includes("profile") && !url.includes("profile/id")) {
        profile.classList.add("active");
    }
})

const removeAllActives = () => {
    navItems.forEach(item => {
        item.classList.remove("active");
    })
}

navItems.forEach(item => {
    item.addEventListener("click", (e) => {
        const active = document.querySelector(".active");
        const dropdown = item.querySelector(".dropdown");
        if (active && !dropdown) {
            e.preventDefault(); // Prevent the default link behavior
            active.classList.add('fade-out'); // Add the fade-out class to initiate the animation
            const a = item.querySelector("a"); //select the <a> in the item
            setTimeout(function () {
                window.location.href = a.href; // Navigate to the link's href after a delay
            }, 200); // Adjust the duration of the fade-out animation (in milliseconds) and the delay before changing the href as needed }
        }
    });
})