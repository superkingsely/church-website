Sure! On a new laptop, configure Git once with your GitHub identity.

## 1. Set your name

Use the name you want to appear on your commits.

```bash
git config --global user.name "Onwumelu Chijioke"
```

or if you prefer another display name:

```bash
git config --global user.name "Master CJ"
```

---

## 2. Set your email

Use the same email that's associated with your GitHub account.

```bash
git config --global user.email "superkingsely@gmail.com"
```

---

## 3. Set the default branch to `main`

```bash
git config --global init.defaultBranch main
```

---

## 4. Enable helpful colored output

```bash
git config --global color.ui auto
```

---

## 5. Enable Git Credential Manager (Windows)

This lets Git securely remember your GitHub login.

```bash
git config --global credential.helper manager
```

---

## 6. Set VS Code as the default editor

```bash
git config --global core.editor "code --wait"
```

---

## 7. Enable automatic line-ending conversion (Windows)

```bash
git config --global core.autocrlf true
```

---

## 8. Verify your configuration

```bash
git config --list
```

or

```bash
git config --global --list
```

You should see something like:

```text
user.name=Onwumelu Chijioke
user.email=superkingsely@gmail.com
init.defaultbranch=main
credential.helper=manager
core.editor=code --wait
core.autocrlf=true
color.ui=auto
```

---

# Your first commit

```bash
git add .

git commit -m "feat: initialize church website solution"
```

---

# Connect to GitHub

If you haven't already:

```bash
git remote add origin https://github.com/YOUR_USERNAME/church-website.git
```

Verify:

```bash
git remote -v
```

---

# Push the first commit

If you're on the `feature/project-setup` branch:

```bash
git push -u origin feature/project-setup
```

Or, if you're pushing `main` for the first time:

```bash
git push -u origin main
```

---

### One recommendation

Since this is a long-term project, I recommend using **SSH** instead of HTTPS for GitHub. Once it's set up, you won't need to enter credentials or tokens when pushing from this laptop.

We can configure SSH after your first push if you haven't already.
