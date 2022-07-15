import cv2
from face_recognition import face_locations, face_encodings, face_distance, compare_faces
from PIL import Image, ImageEnhance
from numpy import asarray
from math import sqrt
from flask import Flask, request, jsonify

def center_of_face(x1, y1, x2, y2):
    """
    Function used to get the center of two diagonal vertices

    :param x1:
    :param y1:
    :param x2:
    :param y2:
    :return: the coordinates of the center
    """

    center_x = int((x1 + x2) * 0.5)
    center_y = int((y1 + y2) * 0.5)
    return center_x, center_y


def get_centermost_face(faces_list, image_dimensions):
    """
    Returns the index of the face
    that is closest to the center of the screen. If more than one share the closest
    distance it picks the first.

    :param faces_list:
    :param image_dimensions:
    :return:
    """

    # image_dimensions is in format (width, height)
    center_of_image = image_dimensions[0]/2, image_dimensions[1]/2

    distances_from_center = []
    for y1, x1, y2, x2 in faces_list:
        face_center = center_of_face(x1, y1, x2, y2)
        center_distance = sqrt((center_of_image[0] - face_center[0])**2 + (center_of_image[1] - face_center[1])**2)
        distances_from_center.append(center_distance)

    center_most_face_ = min(distances_from_center)  # gets the shortest distance
    return distances_from_center.index(center_most_face_)


def get_face_match_percentage(face_encoding1, face_encoding2_):
    face_distances_ = face_distance([face_encoding1], face_encoding2_)
    face_match_percentage_ = (1 - face_distances_) * 100
    face_match_percentage_ = round(face_match_percentage_[0], 2)
    return face_match_percentage_


def draw_rectangles(image, faces_list, main_face, highlight_main_face, main_face_color=(0, 255, 0),
                    other_face_color=(255, 255, 255)):
    index = -1
    for y1, x1, y2, x2 in faces_list:
        index += 1
        if index == main_face:
            if highlight_main_face:
                box_color = main_face_color
            else:
                box_color = (0, 0, 255)
        else:
            box_color = other_face_color
        cv2.rectangle(image, (x1, y1), (x2, y2), box_color, 2)

    return  # center_most_face


def label_face(image, face_coords, text, text_color_=(255, 255, 255), position="bottom", font_scale=0.65):
    y1_, x1_, y2_, x2_ = face_coords

    if position == 'bottom':
        text_position = (x2_, y2_ + 20)
    elif position == 'top':
        text_position = (x2_, y1_)
    else:
        print('Invalid position argument, reverting to default, bottom')
        text_position = 'bottom'

    cv2.putText(
        img=image,
        text=text,
        org=text_position,
        fontFace=cv2.FONT_HERSHEY_TRIPLEX,
        fontScale=font_scale,
        color=text_color_,
        thickness=1
    )


def resize_image(image, base_width_):
    resize_ratio = base_width_ / float(image.shape[1])
    # small_image = cv2.resize(image, (0, 0), fx=resize_ratio, fy=resize_ratio)
    # return small_image
    pil_image = Image.fromarray(image)
    small_image = pil_image.resize((base_width_, int(resize_ratio * image.shape[0])))
    arr_image = asarray(small_image)
    return arr_image


def show_center(image, image_dimensions_, icon_color_=(255, 255, 255), icon_size_=5):
    center = (int(image_dimensions_[0]/1), int(image_dimensions_[1]/1))

    horizontal_start = (center[0]-icon_size_, center[1])
    horizontal_end = (center[0]+icon_size_, center[1])
    cv2.line(
        img=image,
        pt1=horizontal_start,
        pt2=horizontal_end,
        color=icon_color_,
        thickness=1,
        lineType=cv2.LINE_AA,
        shift=1)

    vertical_start = (center[0], center[1] - icon_size_)
    vertical_end = (center[0], center[1] + icon_size_)
    cv2.line(
        img=image,
        pt1=vertical_start,
        pt2=vertical_end,
        color=icon_color_,
        thickness=1,
        lineType=cv2.LINE_AA,
        shift=1)


def enhance_brightness(image, factor):
    pil_image = Image.fromarray(image)
    brightness_enhancer = ImageEnhance.Brightness(pil_image)
    im_output = brightness_enhancer.enhance(factor)
    arr_image = asarray(im_output)
    return arr_image


def enhance_contrast(image, factor):
    pil_image = Image.fromarray(image)
    contrast_enhancer = ImageEnhance.Contrast(pil_image)
    im_output = contrast_enhancer.enhance(factor)
    arr_image = asarray(im_output)
    return arr_image


def transform_image(image_bytes, image_dimensions):
    new_image = Image.frombuffer(mode='RGB', size=image_dimensions, data=image_bytes)
    return new_image

app = Flask(__name__)
@app.get("/greeting")
def hello():
    return "Hello world"

@app.post("/verify")
def verify_face():
    if request.is_json:
        req = request.get_json()
        image1_path = f'../Files/{req["fileName"]}.jpg'
        base_width = 500
        image1 = cv2.imread(image1_path)

        image1 = resize_image(image1, base_width)

        faces_image1 = face_locations(image1)
        face_encodings_image1 = face_encodings(image1)

        image1_dimensions = (image1.shape[1], image1.shape[0])
        centermost_face_image1 = get_centermost_face(faces_image1, image1_dimensions)

        draw_rectangles(image1, faces_image1, centermost_face_image1, True)
        face1_encoding = face_encodings_image1[centermost_face_image1]

        verify_step = vs = 5
        face_tolerance = 0.49
        #read from the bytes
        image_bytes = req["data"]
        image2_dimensions = req['dimensions']
        image2 = transform_image(image_bytes)
        image2 = resize_image(image2, base_width)
        image2_dimensions = (image2.shape[1], image2.shape[0])

        image1 = cv2.cvtColor(image1, cv2.COLOR_BGR2RGB)
        image2 = cv2.cvtColor(image2, cv2.COLOR_BGR2RGB)
        faces_image2 = face_locations(image2)
        face_encodings_image2 = face_encodings(image2)
        centermost_face_image2 = get_centermost_face(faces_image2, image2_dimensions)

        if len(faces_image1) == 0 or len(faces_image2) == 0:
            if len(faces_image1) == 0:
                print("No faces detected in the first image")
            if len(faces_image2) == 0:
                print("No faces detected in the second image")

        else:
            face_encoding2 = face_encodings_image2[centermost_face_image2]
            face_match_bool = compare_faces([face1_encoding], face_encoding2, tolerance=face_tolerance)[0]
            print("Found {0} faces in the first image".format(len(faces_image1)))
            print("Found {0} faces in the second image".format(len(faces_image2)))
    return jsonify(face_match_bool)
